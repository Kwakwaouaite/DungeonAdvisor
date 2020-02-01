using UnityEngine;

public class UIDrag : MonoBehaviour
{
    [SerializeField] UIItem m_Item;
    [SerializeField] Transform m_Root;

    Vector3 m_RootPos;
    Vector3 m_InitDrag;
    Vector3 m_DragOffset = Vector3.zero;
    bool m_Dragging = false;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Item)
        {
            Instantiate<UIItem>(m_Item, m_Root);
        }

        m_RootPos = m_Root.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        m_Root.localPosition = m_RootPos + m_DragOffset;

        if (!m_Dragging)
        {
            m_DragOffset *= 0.9f;
        }
    }

    private void OnMouseOver()
    {
        m_Root.localScale = 1.5f * Vector3.one;
    }

    private void OnMouseExit()
    {
        m_Root.localScale = Vector3.one;
    }

    private void OnMouseDrag()
    {
        m_Dragging = true;

        m_DragOffset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - m_InitDrag;
    }
    private void OnMouseDown()
    {
        m_InitDrag = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseUp()
    {
        m_Dragging = false;
    }


}
