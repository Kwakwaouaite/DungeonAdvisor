using UnityEngine;

public class UICell : MonoBehaviour
{
    [SerializeField] SpriteRenderer m_Visual;
    [SerializeField] SpriteRenderer m_Hover;
    [SerializeField] UIGrid         m_Room;


    int m_Damage = 0;

    // Start is called before the first frame update
    void Start()
    {
        BoxCollider2D col2D = GetComponent<BoxCollider2D>();

        Vector2 colSize = new Vector2();
        colSize.x = m_Room.getCellW();
        colSize.y = m_Room.getCellH();

        col2D.size = colSize;

        m_Hover.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!m_Room)
        {
            return;
        }

        int damage = getDamage();
        if (damage==0)
        {
            return;
        }

        m_Visual.sprite = m_Room.GetRoomConfig().m_BrokenSet[damage];

        
    }

    private void OnMouseOver()
    {
        Debug.Log("Over " + name);
        m_Hover.gameObject.SetActive(true);
    }

    public void OnMouseDown()
    {
        m_Damage++;
        if (m_Damage == m_Room.GetRoomConfig().m_BrokenSet.Length)
        {
            m_Damage = 0;
        }
    }

    private void OnMouseExit()
    {
        m_Hover.gameObject.SetActive(false);
    }



    int getDamage() { return m_Damage; }
}
