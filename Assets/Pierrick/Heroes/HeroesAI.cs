using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesAI : MonoBehaviour
{
    [SerializeField] private SpeechBubble speechBubble;
    [SerializeField] Transform m_Root;

    public HeroesConfig m_HeroesConfig;
    public int m_Happiness = -1;

    private bool m_ReachedExit;
    private List<Vector2Int> m_ItemVisited;


    public Vector2Int m_CurrentPos;

    public void SetOffset(float x, float  y)
    {
        Vector3 localPos = new Vector3(x, y, m_Root.localPosition.z);

        m_Root.localPosition = localPos;
    }

    public float GetPercentageHappiness()
    {
        float clampedHappinnes = Mathf.Clamp(m_Happiness, m_HeroesConfig.MinHappiness, m_HeroesConfig.MaxHappiness);
        float range = m_HeroesConfig.MaxHappiness - m_HeroesConfig.MinHappiness;
        return clampedHappinnes / range;
    }

    public IEnumerator UseObject(UIGrid room, Vector2Int objectPos)
    {
        UIItem item = room.GetItem(objectPos);
        if (item)
        {
            if (item.Available())
            {
                item.Use();
                Debug.Log("Happy");
                m_Happiness += m_HeroesConfig.HappinessUsingObject;

                if (speechBubble)
                {
                    StartCoroutine(speechBubble.SaySomething(SpeechBubble.EReactionType.Happy));
                }

            }
            else
            {

                Debug.Log("Not happy");
                m_Happiness += m_HeroesConfig.HappinessUsingObject;
                if (speechBubble)
                {
                    StartCoroutine(speechBubble.SaySomething(SpeechBubble.EReactionType.Sad));
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator Move(List<Vector2Int> path, UIGrid room, float Speed = 0.5f)
    {

        float wRatio = room.getCellW();
        float hRatio = room.getCellH();

        for (int i = 1; i < path.Count; i++ )
        {
            Vector2Int segment = path[i];

            room.WalkerOn(path[i - 1]);

            Vector2 direction = path[i] - path[i - 1];
            Vector2 currentPosition = path[i - 1];

            float currentPercentage = 0;
            while (currentPercentage < 1)
            {
                float deltaPercentage = Time.deltaTime / Speed;
                currentPercentage += deltaPercentage;
                currentPosition += direction * deltaPercentage;
                gameObject.transform.position = room.transform.position + new Vector3(currentPosition.x * wRatio, currentPosition.y * hRatio);
                yield return null;
            }

            m_CurrentPos = path[i];
        }
    }
    
    public IEnumerator ExploreRoom(UIGrid room, Vector2Int start)
    {
        if (GameManager.IsGroupExploring())
        {
            yield break;
        }

        GameManager.StartGroupExploring();

        m_ReachedExit = false;
        m_ItemVisited = new List<Vector2Int>();

        m_Happiness = m_HeroesConfig.StartHappiness;

        m_CurrentPos = start;

        yield return ScaleUp();

        while (!m_ReachedExit)
        {
            m_ItemVisited.Add(m_CurrentPos);

            Pathfinding.ObjectPathData nextObj = null;

            int currentWaitStep = 0;

           while (currentWaitStep < m_HeroesConfig.m_MaxWaitingStep && nextObj == null)
            {
                Debug.Log("Wait step: " + currentWaitStep);
                nextObj = ChooseNextObject(room);

                if (nextObj == null)
                {
                    if (speechBubble)
                    {
                        StartCoroutine(speechBubble.SaySomething(SpeechBubble.EReactionType.Timer1 + currentWaitStep, m_HeroesConfig.m_MaxWaitingTimes / m_HeroesConfig.m_MaxWaitingStep));
                    }
                    yield return new WaitForSeconds(m_HeroesConfig.m_MaxWaitingTimes / m_HeroesConfig.m_MaxWaitingStep);
                    currentWaitStep++;
                }
            }

            if (nextObj != null)
            {
                yield return Move(nextObj.path, room);

                if (nextObj.objectFound == UIItem.eType.Door)
                {
                    m_ReachedExit = true;
                }
                else
                {
                    yield return UseObject(room, m_CurrentPos);
                }
            }
            else
            {
                // No path found
                m_Happiness += m_HeroesConfig.HappinessCannotLeave;
                m_ReachedExit = true;
            }
        }

        yield return ScaleDown();

        GameManager.StopGroupExploring();
        float happiness = GetPercentageHappiness();
        Debug.Log("Exited with happiness: " + happiness);

    }

    public IEnumerator ScaleUp()
    {
        float time = 0.0f;

        while ( time < 1)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                time = 1;
            }

            float scale = Tween.SineEaseIn(time, 0.0f, 1.0f, 1.0f);

            m_Root.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    public IEnumerator ScaleDown()
    {
        float time = 0.0f;

        while (time < 1)
        {
            time += Time.deltaTime;
            if (time > 1)
            {
                time = 1;
            }

            float scale = 1.0f - Tween.ExpoEaseOut(time, 0.0f, 1.0f, 1.0f);

            m_Root.localScale = Vector3.one * scale;
            yield return null;
        }
    }

    private Pathfinding.ObjectPathData ChooseNextObject(UIGrid room)
    {
        List<Pathfinding.ObjectPathData> nearObjects = room.GetAllPathesFrom(m_CurrentPos);

        Pathfinding.ObjectPathData nextObj = null;

        foreach (Pathfinding.ObjectPathData obj in nearObjects)
        {
            if (!m_ItemVisited.Contains(obj.path[obj.path.Count - 1]))
            {
                // Si on a pas d'objet ou si on a trouvé une porte et que le nouvel objet n'est pas une porte
                if (nextObj == null
                    || (nextObj.objectFound == UIItem.eType.Door
                            && obj.objectFound != UIItem.eType.Door))
                {
                    nextObj = obj;
                }

                if (nextObj.objectFound != UIItem.eType.Door)
                {
                    break;
                }
            }
        }

        return nextObj;
    }

}
