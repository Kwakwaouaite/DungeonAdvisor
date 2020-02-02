using UnityEngine;

public class UIGold : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro m_txtValue;

    float m_Value;

    // Start is called before the first frame update
    void Start()
    {
        m_Value = GameManager.GetGold();
        m_txtValue.SetText(m_Value.ToString("### ### ###"));
    }

    // Update is called once per frame
    void Update()
    {
        m_Value = Mathf.Lerp(m_Value, (float)GameManager.GetGold(), 0.8f);
        m_txtValue.SetText(m_Value.ToString("### ### ###"));
    }
}
