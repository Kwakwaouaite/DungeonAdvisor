using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroesAI : MonoBehaviour
{
    private bool m_ReachedExit;
    private List<Vector2Int> m_ItemVisited;

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
        }
    }
    
    public IEnumerator ExploreRoom(UIGrid room, Vector2Int start)
    {
        m_ReachedExit = false;
        m_ItemVisited = new List<Vector2Int>();

        Vector2Int currentPos = start;

        while (!m_ReachedExit)
        {
            m_ItemVisited.Add(currentPos);
            List<Pathfinding.ObjectPathData> nearObjects = room.GetAllPathesFrom(currentPos);

            Pathfinding.ObjectPathData nextObj = null;

            foreach (Pathfinding.ObjectPathData obj in nearObjects)
            {
                if (!m_ItemVisited.Contains(obj.path[obj.path.Count - 1]))
                {
                    // Si on a pas d'objet ou si on a trouvé une porte et que le nouvel objet n'est pas une porte
                    if (nextObj ==null
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

            if (nextObj != null)
            {
                yield return Move(nextObj.path, room);
                currentPos = nextObj.path[nextObj.path.Count - 1];

                if (nextObj.objectFound == UIItem.eType.Door)
                {
                    m_ReachedExit = true;
                }
            }
            else
            {
                m_ReachedExit = true;
            }
        }
    }

}
