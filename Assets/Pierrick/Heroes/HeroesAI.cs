using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesAI : MonoBehaviour
{
    [SerializeField] private SpeechBubble speechBubble;

    private bool m_ReachedExit;
    private List<Vector2Int> m_ItemVisited;

    public Vector2Int m_CurrentPos;

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

            room.WalkerOn(path[i - 1]);
            m_CurrentPos = path[i];
        }
    }
    
    public IEnumerator ExploreRoom(UIGrid room, Vector2Int start)
    {
        m_ReachedExit = false;
        m_ItemVisited = new List<Vector2Int>();

        m_CurrentPos = start;

        while (!m_ReachedExit)
        {
            m_ItemVisited.Add(m_CurrentPos);
            List<Pathfinding.ObjectPathData> nearObjects = room.GetAllPathesFrom(m_CurrentPos);

            Pathfinding.ObjectPathData nextObj = ChooseNextObject(nearObjects);

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
