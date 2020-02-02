using UnityEngine;

public class UICell : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_Visual;
    [SerializeField] SpriteRenderer m_Hover;
    [SerializeField] UIGrid         m_Room;
    [SerializeField] UIItem         m_Item;

    public int m_X;
    public int m_Y;
    int m_Damage = 0;
    public int m_Liquid = 0;

    public Vector2 GetPosition()
    {
        return new Vector2(m_X, m_Y);
    }

    public UIItem.eType GetEType()
    {
        if (m_Item != null)
        {
            return m_Item.getItemType();
        }

        return UIItem.eType.None;
    }

    public UIItem GetUIItem()
    {
        return m_Item;
    }

    public bool GetWalkable()
    {
        return m_Damage < GetMaxDamage();
    }

    public void SetDamage(int damage)
    {
        m_Damage = Mathf.Clamp(damage, 0, GetMaxDamage());
    }

    public void DoDamage(int damage = 1)
    {
        m_Damage = Mathf.Clamp(m_Damage + damage, 0, GetMaxDamage());
    }

    public int GetMaxDamage()
    {
        return m_Room.GetRoomConfig().m_BrokenSet.Length;
    }

    public bool isFullBroken()
    {
        return m_Damage == GetMaxDamage();
    }

    public bool isDamaged()
    {
        return m_Damage != 0;
    }

    public bool isLiquid()
    {
        return m_Liquid != 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D col2D = GetComponent<BoxCollider2D>();

        Vector2 colSize = new Vector2();
        colSize.x = m_Room.getCellW();
        colSize.y = m_Room.getCellH();

        col2D.size = colSize;

        m_Hover.gameObject.SetActive(false);
        
        m_Visual.flipX = Random.Range(0, 127) < 64;
        m_Visual.flipY = Random.Range(0, 127) < 64;
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
                m_Visual.sprite = null;
                return;

            }

        }

        if (isLiquid())
        {
            m_Visual.sprite = m_Room.GetRoomConfig().m_LiquidSet[m_Liquid - 1];
        }
        else
        {
            int damage = getDamage();
            if (damage == 0)
            {
                m_Visual.sprite = null;
            }
            else
            {
                m_Visual.sprite = m_Room.GetRoomConfig().m_BrokenSet[damage - 1];
            }

            m_Hover.color = m_Room.GetRoomConfig().m_HoverColor[damage];
        }

    }

    public bool canHover()
    {
        return (!m_Item)
            || m_Item.getItemType() != UIItem.eType.Door;
    }
    private void OnMouseOver()
    {
        if (canHover())
        {
          //  Debug.Log("Over " + name);
            m_Hover.gameObject.SetActive(true);
            GameManager.SetActiveCell(this);
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
        GameManager.ResetActiveCell(this);

    }

    int getDamage() { return m_Damage; }
    public bool canRepair() { return m_Item == null;  }


    public void AddItem(UIItem item) { m_Item = item;  }
    public void SetCoord(int x, int y) { m_X = x; m_Y = y; }

}
