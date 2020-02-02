using UnityEngine;

public class UITimer : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshPro m_Value;
    [SerializeField] Animator m_HourglassAnim;
    [SerializeField] UIVignette m_Vignette;
    [SerializeField] float m_StressedValue = 2.0f;
    [SerializeField] AudioSource slowSfx;
    [SerializeField] AudioSource stressedSfx;


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
            slowSfx.enabled = false;
            stressedSfx.enabled = false;

            m_Time = 0;
            m_Value.SetText("0.0");

            m_Value.color = m_Off;
            m_HourglassAnim.enabled = false;

            m_Vignette.SetColor(UIVignette.eColor.TimerOff);
        }
        else
        {

            m_Time -= Time.deltaTime;
            m_Value.SetText(m_Time.ToString("##.#"));

            if (m_Time > m_StressedValue)
            {

                slowSfx.enabled = true;
                stressedSfx.enabled = false;


                m_Vignette.SetColor(UIVignette.eColor.TimerOn);

                m_Value.color = m_Good;
            }
            else
            {
                slowSfx.enabled = false;
                stressedSfx.enabled = true;

                m_Vignette.SetColor(UIVignette.eColor.TimerStress);

                m_Value.color = m_Stressed;
            }
        }

    }
}
