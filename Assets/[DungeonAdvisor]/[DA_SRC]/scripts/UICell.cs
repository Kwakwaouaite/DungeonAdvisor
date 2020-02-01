using UnityEngine;

public class UICell : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_Visual;
    [SerializeField] SpriteRenderer m_Hover;
    [SerializeField] UIGrid         m_Room;
    [SerializeField] UIItem         m_Item;

    int m_X;
    int m_Y;
    int m_Damage ;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D col2D = GetComponent<BoxCollider2D>();

        Vector2 colSize = new Vector2();
        colSize.x = m_Room.getCellW();
        colSize.y = m_Room.getCellH();

        col2D.size = colSize;

        m_Hover.gameObject.SetActive(false);

        int max = m_Room.GetRoomConfig().m_BrokenSet.Length + 1;
        m_Damage = Random.Range(0, max);
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Room)
        {
            return;
        }

        if (m_Item != null)
        {
            if (m_Item.getItemType() == UIItem.eType.Door)
            {
                m_Damage = 0;
            }
        }
        int damage = getDamage();
        if (damage==0)
        {
            m_Visual.sprite = null;
        }
        else
        {
            m_Visual.sprite = m_Room.GetRoomConfig().m_BrokenSet[damage-1];
        }
        m_Hover.color = m_Room.GetRoomConfig().m_HoverColor[damage];

    }

    bool canHover()
    {
        return (!m_Item)
            || m_Item.getItemType() != UIItem.eType.Door;
    }
    private void OnMouseOver()
    {
        if (canHover())
        {
            Debug.Log("Over " + name);
            m_Hover.gameObject.SetActive(true);
        }
    }

    public void OnMouseDown()
    {
        if (m_Item)
        {
            m_Item.Repair();
            return;
        }

        if (canRepair() && m_Damage==0)
        {
            return;
        }

        m_Damage--;
       
    }

    private void OnMouseExit()
    {
        m_Hover.gameObject.SetActive(false);
    }

    int getDamage() { return m_Damage; }
    bool canRepair() { return m_Item == null;  }

    public void AddItem(UIItem item) { m_Item = item;  }
    public void SetCoord(int x, int y) { m_X = x; m_Y = y; }

}
