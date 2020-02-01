using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    public DA_Object m_Object;

    public virtual bool IsWalkable()
    {
        return true;
    }

    public virtual bool HaveStuff()
    {
        return m_Object != null;
    }
}

public class BrokenFloor : Cell
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

    private void OnDrawGizmos()
    {
        Vector3 offset;
        Gizmos.color = Color.blue;
        for (int i = 0; i < m_Width+1; i++)
        {
            offset = Vector3.right * i;
            Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.up * m_Height);
        }

        for (int i = 0; i < m_Height + 1; i++)
        {
            offset = Vector3.up * i;
            Gizmos.DrawLine(transform.position + offset, transform.position + offset + Vector3.right * m_Width);
        }

        if (!Application.IsPlaying(this))
        {
            return;
        }

        offset = Vector3.right * 0.5f + Vector3.up * 0.5f + Vector3.forward;

        for (int i = 0; i < m_Cells.GetLength(0); i++)
        {
            for (int j = 0; j < m_Cells.GetLength(1); j++)
            {
                Gizmos.color = m_Cells[i, j].IsWalkable() ? (m_Cells[i, j].HaveStuff() ? Color.yellow : Color.green) : Color.red;

                Gizmos.DrawSphere(transform.position + offset + Vector3.right * i + Vector3.up * j, 0.5f);
            }
        }

        if (d_foundObjects != null)
        {
            offset = transform.position + Vector3.right * 0.5f + Vector3.up * 0.5f;

            foreach (Pathfinding.ObjectPathData objectPathData in d_foundObjects)
            {
                Gizmos.color = Color.red;

                for (int i = 1; i < objectPathData.path.Count; i++)
                {
                    Gizmos.DrawLine(new Vector3(objectPathData.path[i - 1].x, objectPathData.path[i - 1].y) + offset
                        , new Vector3(objectPathData.path[i].x, objectPathData.path[i].y) + offset);
                }
            }
        }
    }

    private void Awake()
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
                        m_Cells[i, j].m_Object = new DA_Object();
                    }
                }
                else
                {
                    m_Cells[i, j] = new BrokenFloor();
                }
            }
        }

        List<Pathfinding.ObjectPathData> found_Object = Pathfinding.FindAccessibleObjects(this, Vector2Int.zero);
        d_foundObjects = found_Object;
    }

    // Start is called before the first frame update
    void Start()
    {

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
