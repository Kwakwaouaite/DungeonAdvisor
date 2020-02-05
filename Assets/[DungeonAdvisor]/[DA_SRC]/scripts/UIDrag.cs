using UnityEngine;

public class UIDrag : MonoBehaviour
{
    [SerializeField] UIItem m_Item;
    [SerializeField] Transform m_Root;

    Vector3 m_RootPos;
    Vector3 m_InitDrag;
    Vector3 m_DragOffset = Vector3.zero;
    bool m_Dragging = false;
    Vector3 m_SnapPos;
    bool m_SnapToPos;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Item)
        {
            Instantiate<UIItem>(m_Item, m_Root);
        }
        else
        {
            enabled = false;
        }

        m_RootPos = m_Root.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 nextPos = m_RootPos + m_DragOffset;

        if (m_SnapToPos)
        {
            nextPos = Vector3.Lerp(nextPos, m_SnapPos, 0.9f);
        }

        m_Root.localPosition = nextPos;

        if (!m_Dragging)
        {
            m_DragOffset *= 0.9f;
        }
    }

    private void OnMouseOver()
    {
        if (GameManager.CanAddItems())
        {
            m_Root.localScale = 1.5f * Vector3.one;
        }
    }

    private void OnMouseExit()
    {
        m_Root.localScale = Vector3.one;
    }

    bool isCellValid(UICell cell)
    {
        return (
            GameManager.CanAddItems() &&
            m_Item.CanBuy() &&
            cell &&
            cell.canHover() &&
            cell.GetEType() == UIItem.eType.None &&
            (!cell.isDamaged()) &&
            (!cell.isLiquid())
            );
    }
    private void OnMouseDrag()
    {
        if (!GameManager.CanAddItems())
        {
            return;
        }
        m_Dragging = true;

        m_DragOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_InitDrag;
        UICell activeCell = GameManager.GetActiveCell();

        if (activeCell && isCellValid(activeCell))
        {
            m_SnapToPos = true;
            m_SnapPos = activeCell.transform.position - transform.position;
        }
        else
        {
            m_SnapToPos = false;
        }
    }
    private void OnMouseDown()
    {
        m_InitDrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        m_Dragging = false;

        if (GameManager.CanAddItems())
        {
            UICell activeCell = GameManager.GetActiveCell();

            if (activeCell && isCellValid(activeCell))
            {
                UIItem item = Instantiate<UIItem>(m_Item, activeCell.transform);
                activeCell.AddItem(item);
                m_DragOffset = Vector3.zero;

                m_Item.Buy();
            }
        }
        m_SnapToPos = false;

    }


}
