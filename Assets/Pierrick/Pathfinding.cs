using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public class ObjectPathData
    {
        public UIItem.eType objectFound;
        public List<Vector2Int> path;
    }

    public static readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    static public List<ObjectPathData> FindAccessibleObjects(DA_Grid grid, Vector2Int StartingPoint)
    {
        List<ObjectPathData> ObjectFound = new List<ObjectPathData>();

        Queue<Vector2Int> NextIteration = new Queue<Vector2Int>();
        Queue<Vector2Int> CurrentIteration = new Queue<Vector2Int>();

        ObjectPathData[,] fakeGrid = new ObjectPathData[grid.m_Width, grid.m_Height];

        NextIteration.Enqueue(StartingPoint);
        fakeGrid[StartingPoint.x, StartingPoint.y] = new ObjectPathData();
        fakeGrid[StartingPoint.x, StartingPoint.y].path = new List<Vector2Int>();
        fakeGrid[StartingPoint.x, StartingPoint.y].path.Add(StartingPoint);


        while( NextIteration.Count > 0)
        {
            CurrentIteration = NextIteration;
            NextIteration = new Queue<Vector2Int>();

            while (CurrentIteration. Count > 0)
            {
                Vector2Int currentNode = CurrentIteration.Dequeue();

                List<Vector2Int> randomDirection = new List<Vector2Int>(Directions);

                ShuffleDirection(randomDirection);

                foreach (Vector2Int direction in randomDirection)
                {
                    Vector2Int potentialNode = currentNode + direction;

                    if (IsValidPosition(potentialNode, grid))
                    {
                        if (fakeGrid[potentialNode.x, potentialNode.y] != null)
                        {
                            continue;
                        }

                        ObjectPathData newPathData = new ObjectPathData();
                        newPathData.path = new List<Vector2Int>();
                        Cell potentialCell = grid.m_Cells[potentialNode.x, potentialNode.y];

                        if (potentialCell.IsWalkable())
                        {
                            NextIteration.Enqueue(potentialNode);

                            newPathData.path.AddRange(fakeGrid[currentNode.x, currentNode.y].path);
                            newPathData.path.Add(potentialNode);

                            newPathData.objectFound = potentialCell.m_Object;

                            if (newPathData.objectFound != UIItem.eType.None)
                            {
                                ObjectFound.Add(newPathData);
                            }
                        }

                        fakeGrid[potentialNode.x, potentialNode.y] = newPathData;
                    }
                }

            }
        }

        return ObjectFound;
    }

    private static void ShuffleDirection (List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(0, list.Count);
            Vector2Int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    private static bool IsValidPosition(Vector2 position, DA_Grid grid)
    {
        if (0 <= position.x && position.x < grid.m_Width
            && 0 <= position.y && position.y < grid.m_Height)
        {
            return true;
        }

        return false;
    }
}
