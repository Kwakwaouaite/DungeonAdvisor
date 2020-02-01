using UnityEngine;

public class UIItem : MonoBehaviour
{
    public enum eType
    {
        None,
        Door,
        Chest,
        Enemy,
        Trap
    };

    [SerializeField] eType m_Type = eType.None;
    [SerializeField] GameObject m_Visual;
    [SerializeField] GameObject m_VisualBroken;
    [SerializeField] bool m_Broken;
    [SerializeField] int m_Cost;

    // Start is called before the first frame update
    void Start()
    {
        RefreshState();
    }

    public void Use()
    {
        m_Broken = true;
        RefreshState();
    }

    public void Repair()
    {
        m_Broken = false;
        RefreshState();
    }

    private void Update()
    {
        RefreshState();
    }

    void RefreshState()
    {
        m_Visual.SetActive(!m_Broken);
        m_VisualBroken.SetActive(m_Broken);
    }

    public eType getItemType() { return m_Type;  }
}
