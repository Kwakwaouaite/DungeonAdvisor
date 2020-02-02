using UnityEngine;

public class UITimer : MonoBehaviour
{

    [SerializeField] TMPro.TextMeshPro m_Value;
    [SerializeField] Animator m_HourglassAnim;

    public Color m_Off;
    public Color m_Good;
    public Color m_Stressed;

    float m_Time = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_HourglassAnim.enabled = false;
    }

    public void SetTimer(float duration)
    {
        m_Time = duration;
        m_HourglassAnim.enabled = true;

    }

    private void Update()
    {
        if (m_Time <= 0)
        {
            m_Time = 0;
            m_Value.SetText("0.0");

            m_Value.color = m_Off;
            m_HourglassAnim.enabled = false;
        }
        else
        {

            m_Time -= Time.deltaTime;
            m_Value.SetText(m_Time.ToString("##.#"));

            if (m_Time > 10.0)            m_Value.color = m_Good;
            else
            {
                m_Value.color = m_Stressed;
            }
        }

    }
}
