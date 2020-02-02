using UnityEngine;

public class UIItem : MonoBehaviour
{
    public enum eType
    {
        None,
        Door,
        Chest,
        Enemy,
        Trap,
        Torch,
        Princess,
        Potion,
        Helmet,
        Jar,
        Gold,
        Count
    };

    [SerializeField] eType m_Type = eType.None;
    [SerializeField] GameObject m_Visual;
    [SerializeField] GameObject m_VisualBroken;
    [SerializeField] bool m_Broken;
    [SerializeField] int m_BuyCost;
    [SerializeField] int m_RepairCost;

    // Start is called before the first frame update
    void Start()
    {
        RefreshState();
    }

    public bool Available()
    {
        return !m_Broken;
    }

    public void Use()
    {
        m_Broken = true;
        RefreshState();
    }

    public bool CanBuy()
    {
        return (GameManager.HasEnoughGold(m_BuyCost));
    }

    public void Buy()
    {
        if (CanBuy())
        {
            m_Broken = false;
            GameManager.DecGold(m_BuyCost);
            RefreshState();
        }
    }

    public bool CanRepair()
    {
        return (GameManager.HasEnoughGold(m_RepairCost));
    }

    public void Repair()
    {
        if (CanRepair() && m_Broken)
        {
            m_Broken = false;
            GameManager.DecGold(m_RepairCost);
            RefreshState();
        }
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
