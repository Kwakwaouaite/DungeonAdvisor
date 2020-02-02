using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesAI : MonoBehaviour
{
    [SerializeField] private SpeechBubble speechBubble;
    [SerializeField] private static int waitingStep = 5;
    [SerializeField] private static float waitingTimes = 1.0f;
    [SerializeField] Transform m_Root;

    private bool m_ReachedExit;
    private List<Vector2Int> m_ItemVisited;


    public Vector2Int m_CurrentPos;

    public void SetOffset(float x, float  y)
    {
        Vector3 localPos = new Vector3(x, y, m_Root.localPosition.z);

        m_Root.localPosition = localPos;
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
                if (speechBubble)
                {
                    StartCoroutine(speechBubble.SaySomething(SpeechBubble.EReactionType.Happy));
                }

            }
            else
            {

                Debug.Log("Not happy");
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
        m_ReachedExit = false;
        m_ItemVisited = new List<Vector2Int>();

        m_CurrentPos = start;

        StartCoroutine(ScaleUp());

        while (!m_ReachedExit)
        {
            m_ItemVisited.Add(m_CurrentPos);
            List<Pathfinding.ObjectPathData> nearObjects = room.GetAllPathesFrom(m_CurrentPos);

            Pathfinding.ObjectPathData nextObj = null;

            int waitStep = 0;

            while (wai)

                nextObj = ChooseNextObject(nearObjects);

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
                m_ReachedExit = true;
            }
        }

        yield return ScaleDown();

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

    private Pathfinding.ObjectPathData ChooseNextObject(List<Pathfinding.ObjectPathData> nearObjects)
    {
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
