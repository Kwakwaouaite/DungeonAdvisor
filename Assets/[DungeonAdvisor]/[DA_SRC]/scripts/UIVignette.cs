using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIVignette : MonoBehaviour
{

    public enum eColor
    {
        TimerOff,
        TimerOn,
        TimerStress
    };

    [SerializeField] Color m_TimerOff;
    [SerializeField] Color m_TimerOn;
    [SerializeField] Color m_TimerStressed;
    [SerializeField] SpriteRenderer m_Vignette;

    Color m_Color;
    Color m_DestColor;

    // Start is called before the first frame update
    void Start()
    {
        m_Color = m_Vignette.color;
        m_DestColor = m_Color;
    }

    public void SetColor(eColor col)
    {
        switch(col)
        {
            case eColor.TimerOn: m_DestColor = m_TimerOn; break;
            case eColor.TimerOff: m_DestColor = m_TimerOff; break;
            case eColor.TimerStress: m_DestColor = m_TimerStressed; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        m_Color.a = m_Vignette.color.a;
        m_Color = Color.Lerp(m_Color, m_DestColor, 0.8f);
        m_Vignette.color = m_Color;
    }
}
