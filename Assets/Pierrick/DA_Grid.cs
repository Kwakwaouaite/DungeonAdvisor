using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public UIItem.eType m_Object;
    public bool m_Walkable = true;

    public virtual bool IsWalkable()
    {
        return m_Walkable;
    }

    public virtual bool HaveStuff()
    {
        return m_Object != UIItem.eType.None;
    }
}

public class Wall : Cell
{
    public override bool IsWalkable()
    {
        return false;
    }
}

public class DA_Grid : MonoBehaviour
{
    public int m_Height = 6;
    public int m_Width = 8;

    public Cell[,] m_Cells;

    private List<Pathfinding.ObjectPathData> d_foundObjects;
    [SerializeField] private UIGrid d_UIGrid;

    private void OnDrawGizmosSelected()
    {
        Vector3 offset;
        Gizmos.color = Color.blue;

        float wRatio = 1;
        float hRatio = 1;

        if (d_UIGrid != null)
        {
            wRatio = d_UIGrid.getCellW();
            hRatio = d_UIGrid.getCellH();
        }

        for (int i = 0; i < m_Width+1; i++)
        {
            offset = Vector3.right * i * wRatio;
            Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.up * m_Height * hRatio);
        }

        for (int i = 0; i < m_Height + 1; i++)
        {
            offset = Vector3.up * i * hRatio;
            Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.right * m_Width * wRatio);
        }

        if (!Application.IsPlaying(this))
        {
            return;
        }

        offset = Vector3.right * 0.5f * wRatio + Vector3.up * 0.5f * hRatio + Vector3.forward;

        for (int i = 0; i < m_Cells.GetLength(0); i++)
        {
            for (int j = 0; j < m_Cells.GetLength(1); j++)
            {
                if (m_Cells[i, j] == null)
                {
                    continue;
                }

                Gizmos.color = m_Cells[i, j].IsWalkable() ? (m_Cells[i, j].HaveStuff() ? Color.yellow : Color.green) : Color.red;

                Gizmos.DrawSphere(transform.position + offset + Vector3.right * i * wRatio + Vector3.up * j * hRatio, 0.5f * wRatio);
            }
        }

        if (d_foundObjects != null)
        {
            offset = transform.position + Vector3.right * 0.5f * wRatio + Vector3.up * 0.5f * hRatio;

            foreach (Pathfinding.ObjectPathData objectPathData in d_foundObjects)
            {
                Gizmos.color = Color.red;

                for (int i = 1; i < objectPathData.path.Count; i++)
                {
                    Gizmos.DrawLine(new Vector3(objectPathData.path[i - 1].x * wRatio, objectPathData.path[i - 1].y * hRatio) + offset
                        , new Vector3(objectPathData.path[i].x * wRatio, objectPathData.path[i].y * hRatio) + offset);
                }
            }
        }
    }

    private void Start()
    {
        GenerateRandomGrid();
    }

    private void GenerateRandomGrid()
    {
        m_Cells = new Cell[m_Width, m_Height];

        for (int i = 0; i < m_Cells.GetLength(0); i++)
        {
            for (int j = 0; j < m_Cells.GetLength(1); j++)
            {
                float rand = Random.Range(0.0f, 1.0f);
                if (rand < 0.95f)
                {
                    m_Cells[i, j] = new Cell();
                    float rand2 = Random.Range(0.0f, 1.0f);
                    if(rand2 > 0.9f)
                    {
                        m_Cells[i, j].m_Object = UIItem.eType.Chest;
                    }
                }
                else
                {
                    m_Cells[i, j] = new Wall();
                }
            }
        }

    }

    public void SetCells(Cell [,] grid)
    {
        m_Cells = grid;

        // DEBUG
        List<Pathfinding.ObjectPathData> found_Object = Pathfinding.FindAccessibleObjects(this, new Vector2Int(0, 5));
        d_foundObjects = found_Object;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GenerateRandomGrid();
        }
    }
}
