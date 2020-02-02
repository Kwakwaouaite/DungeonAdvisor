using System.Collections.Generic;
using UnityEngine;

public class UIGrid : MonoBehaviour
{
    [SerializeField] RoomConfig m_RoomConfig;
    [SerializeField] ItemConfig m_ItemConfig;
    [SerializeField] ScreenRefConfig m_ScreenRefConfig;
    [SerializeField] Transform m_Pivot;
    [SerializeField] SpriteRenderer m_Background;
    [SerializeField] UICell m_CellBase;
    [SerializeField] DA_Grid m_IAGrid;

    [SerializeField] HeroesAI d_HeroesAI;

    float getWidth()    { return m_RoomConfig.getWidth() / m_ScreenRefConfig.getWidth();  }
    float getHeight()   { return m_RoomConfig.getHeight() / m_ScreenRefConfig.getHeight();  }
    public float getCellW()    { return m_RoomConfig.m_CellW / m_ScreenRefConfig.getWidth(); }
    public float getCellH()    { return m_RoomConfig.m_CellH / m_ScreenRefConfig.getHeight(); }
    int  getRow() { return (int)m_RoomConfig.m_Row;  }
    int  getColumn() { return (int)m_RoomConfig.m_Column; }

    Vector3 getPos(Vector3 pos)
    {
        return new Vector3(
            pos.x / m_ScreenRefConfig.getWidth(),
            pos.y / m_ScreenRefConfig.getHeight(),
            pos.z);
    }

    Vector3 getRefGridPos()
    {
        return new Vector3
            (
                (-getWidth() + getCellW()) *0.5f ,
                (-getHeight() + getCellH())*0.5f,
                0
            );
    }

    bool isBorder(int i, int j)
    {
        return (i == 0)
            || (j == 0)
            || (i == getColumn() - 1)
            || (j == getRow() - 1);
    }

    public RoomConfig GetRoomConfig() {  return m_RoomConfig; }
    List<UICell> m_Cells;
    int m_CellCount;

    public UIItem GetItem(Vector2Int position)
    {
        foreach (UICell cell in m_Cells)
        {
            if (cell.GetPosition() == position)
            {
                return cell.GetUIItem();
            }
        }

        return null;
    }

    private void Awake()
    {
        Debug.Log("awake");

        m_IAGrid.m_Width  = getColumn();
        m_IAGrid.m_Height = getRow();
    }
    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        Vector2 colSize = new Vector2(getWidth(), getHeight());
        Vector2 colOffs = colSize * 0.5f;

        col.size = colSize;
        col.offset = colOffs;

        d_HeroesAI.SetOffset(getCellW   () *.5f, getCellH());

        Debug.Log("start");
        SetupRoom();
    }

    UICell InstantiateCell(int x, int y, UIItem.eType type)
    {
        Transform parent = m_CellBase.transform.parent;
        Vector3 refPos = getRefGridPos();

        UICell cell = Instantiate<UICell>(m_CellBase, parent);
        cell.transform.localPosition = refPos + getPos(m_RoomConfig.getPos(x, y));
        cell.name = "cell_" + x + "_" + y + ( type != UIItem.eType.None ? "_"+type.ToString() : "");
        cell.gameObject.SetActive(true);
        cell.SetCoord(x, y);

        m_Cells.Add(cell);

        return cell;
    }
    void SetupRoom()
    {
        m_Background.sprite = m_RoomConfig.m_Background;

        if (m_CellBase)
        {
            m_CellCount = m_RoomConfig.getCellCount();
            if (m_CellCount==0)
            {
                m_Cells = null;
            }
            else
            {
                Transform parent = m_CellBase.transform.parent;
                m_Cells = new List<UICell>();
              

                for (int j=1;j<getRow()-1; j++)
                {
                    for (int i = 1; i < getColumn() - 1; i++)
                    {
                        InstantiateCell(i, j, UIItem.eType.None);
                    }
                }

            }
            m_CellBase.gameObject.SetActive(false);

            ApplyRandomDamageToAllCell();

            AddDoors();

            GenerateLogicGrid();

            /*
            if (d_HeroesAI)
            {
                List<Pathfinding.ObjectPathData> found_Object = Pathfinding.FindAccessibleObjects(m_IAGrid, new Vector2Int(0, 5));

                if (found_Object.Count > 0)
                {
                    StartCoroutine(d_HeroesAI.Move(found_Object[0].path, this, 0.5f));
                }
            }*/
        }
    }

    private void ApplyRandomDamageToAllCell()
    {
        foreach (UICell uICell in m_Cells)
        {
            uICell.SetDamage(Random.Range(0, uICell.GetMaxDamage() + 1));
        }
    }

    void AddDoors()
    {
        // add up
        UICell cellU = InstantiateCell(5, m_RoomConfig.GetUp(), UIItem.eType.Door);
        UIItem doorU = Instantiate<UIItem>(m_RoomConfig.GetDoor(RoomConfig.eDir.UP),cellU.transform);
        cellU.AddItem(doorU);

        // add left
        UICell cellL = InstantiateCell(m_RoomConfig.GetLeft(), 5, UIItem.eType.Door);
        UIItem doorL = Instantiate<UIItem>(m_RoomConfig.GetDoor(RoomConfig.eDir.LEFT), cellL.transform);
        cellL.AddItem(doorL);

        // add bottom
        UICell cellB = InstantiateCell(5, m_RoomConfig.GetDown(),UIItem.eType.Door);
        UIItem doorB = Instantiate<UIItem>(m_RoomConfig.GetDoor(RoomConfig.eDir.DOWN), cellB.transform);
        cellB.AddItem(doorB);

        // add right
        UICell cellR = InstantiateCell(m_RoomConfig.GetRight(), 5, UIItem.eType.Door);
        UIItem doorR = Instantiate<UIItem>(m_RoomConfig.GetDoor(RoomConfig.eDir.RIGHT), cellR.transform);
        cellR.AddItem(doorR);
    }


    void AddItems()
    {

    }

    public List<Pathfinding.ObjectPathData> GetAllPathesFrom(Vector2Int start)
    {
        GenerateLogicGrid();
        return Pathfinding.FindAccessibleObjects(m_IAGrid, start);
    }

    public void WalkerOn(Vector2Int tileWalked)
    {
        foreach (UICell cell in m_Cells)
        {
            if (cell.GetPosition() == tileWalked
                && cell.GetEType() == UIItem.eType.None)
            {
                cell.DoDamage(1);
                break;
            }
        }
    }


    private void GenerateLogicGrid()
    {
        Cell[,] newGrid = new Cell[getColumn(), getRow()];

        foreach (UICell uiCell in m_Cells)
        {
            Cell newCell = new Cell();
            newCell.m_Object = uiCell.GetEType();
            newCell.m_Walkable = uiCell.GetWalkable();

            newGrid[uiCell.m_X, uiCell.m_Y] = newCell;
        }

        m_IAGrid.SetCells(newGrid);
    }

    private void Update()
    {
        m_Pivot.localPosition = new Vector3(getWidth() / 2, getHeight() / 2, 0);

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.S))
        {
            LaunchExploreRoom();
            
            //d_HeroesAI
        }
#endif
    }

    public void LaunchExploreRoom()
    {
        StartCoroutine(d_HeroesAI.ExploreRoom(this, new Vector2Int(0, 5)));
    }

    private void OnMouseOver()
    {
        Debug.Log("On grid");
    }

    void OnDrawGizmos()
    {
        if (m_RoomConfig == null)
        {
            return;
        }

        // Draw a yellow sphere at the transform's position
        Vector3 pos = transform.position;
        Vector3 w = new Vector3(getWidth(), 0, 0);
        Vector3 h = new Vector3(0, getHeight(), 0);

        Gizmos.color = Color.yellow;

        Vector3 offset = Vector3.zero;
        float iX = getCellW();
        float iY = getCellH();

        for (int i=0; i<= m_RoomConfig.m_Column;i++)
        {
            Gizmos.DrawLine(pos + offset, pos + h + offset);
            offset.x += iX;
        }

        offset = Vector3.zero;

        for (int i = 0; i <= m_RoomConfig.m_Row; i++)
        {
            Gizmos.DrawLine(pos + offset, pos + w + offset);
            offset.y += iY;
        }
    }


}
