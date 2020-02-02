using UnityEngine;

public class UIButton : MonoBehaviour
{
    [SerializeField] Transform m_Root;

    Vector3 m_RootPos;


    // Start is called before the first frame update
    void Start()
    {       
        m_RootPos = m_Root.localPosition;
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

   
    private void OnMouseDown()
    {
        
    }
   


}
