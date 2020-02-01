using UnityEngine;

public class Cell
{
    DA_Object m_Object;

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

        offset = Vector3.right * 0.5f + Vector3.down * 0.5f;

        for (int i = 0; i < m_Cells.GetLength(0); i++)
        {
            for (int j = 0; j < m_Cells.GetLength(1); j++)
            {
                Gizmos.color = m_Cells[i, j].IsWalkable() ? Color.green : Color.red;

                Gizmos.DrawSphere(transform.position + offset + Vector3.right * i + Vector3.down * j, 0.5f);
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
                if (rand < 0.8f)
                {
                    m_Cells[i, j] = new Cell();
                }
                else
                {
                    m_Cells[i, j] = new BrokenFloor();
                }
            }
        }
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
