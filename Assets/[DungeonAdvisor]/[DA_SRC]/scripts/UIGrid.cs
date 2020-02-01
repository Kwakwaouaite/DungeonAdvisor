using UnityEngine;

[ExecuteInEditMode]
public class UIGrid : MonoBehaviour
{
    [SerializeField] RoomConfig m_RoomConfig;
    [SerializeField] ItemConfig m_ItemConfig;
    [SerializeField] ScreenRefConfig m_ScreenRefConfig;
    [SerializeField] Transform m_Pivot;
    [SerializeField] SpriteRenderer m_Background;
    [SerializeField] UICell m_CellBase;

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
    UICell[] m_Cells;
    int m_CellCount;

    private void Awake()
    {
        Debug.Log("awake");
    }
    // Start is called before the first frame update
    void Start()
    {
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
                m_Cells = new UICell[m_CellCount];
              

                for (int j=1;j<getRow()-1; j++)
                {
                    for (int i = 1; i < getColumn() - 1; i++)
                    {
                        InstantiateCell(i, j, UIItem.eType.None);
                    }
                }

            }
            m_CellBase.gameObject.SetActive(false);

            AddDoors();
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

    private void Update()
    {
        m_Pivot.localPosition = new Vector3(getWidth() / 2, getHeight() / 2, 0);
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
