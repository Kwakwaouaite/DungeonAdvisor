using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public class ObjectPathData
    {
        public DA_Object objectFound;
        public Vector2[] path;
    }

    public static readonly Vector2Int[] Directions = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

    public ObjectPathData[] FindAccessibleObjects(DA_Grid grid, Vector2Int StartingPoint)
    {
        ObjectPathData[] ObjectFound;

        Queue<Vector2Int> NextIteration = new Queue<Vector2Int>();
        Queue<Vector2Int> CurrentIteration = new Queue<Vector2Int>();

        ObjectPathData[,] fakeGrid = new ObjectPathData[grid.m_Width, grid.m_Height];

        NextIteration.Enqueue(StartingPoint);

        while( NextIteration.Count > 0)
        {
            CurrentIteration = NextIteration;
            NextIteration = new Queue<Vector2Int>();

            Vector2Int currentNode = CurrentIteration.Dequeue();

            foreach (Vector2Int direction in Directions)
            {
                Vector2Int potentialNode = currentNode + direction;

                if (IsValidPosition(potentialNode, grid))
                {
                    if (fakeGrid[potentialNode.x, potentialNode.y] != null)
                    {
                        continue;
                    }

                    ObjectPathData newPathData = new ObjectPathData();
                    Cell potentialCell = grid.m_Cells[potentialNode.x, potentialNode.y];

                    if (potentialCell.IsWalkable())
                    {
                        //newPathData
                    }
                }

            }
        }

        return null;
    }

    private bool IsValidPosition(Vector2 position, DA_Grid grid)
    {
        if (0 < position.x && position.x < grid.m_Width
            && 0 < position.y && position.y < grid.m_Height)
        {
            return true;
        }

        return false;
    }
}
