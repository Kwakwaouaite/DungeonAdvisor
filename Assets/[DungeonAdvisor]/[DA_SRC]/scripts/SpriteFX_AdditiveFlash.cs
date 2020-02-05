using UnityEngine;

public class SpriteFX_AdditiveFlash : MonoBehaviour
{
    [SerializeField] float m_AlphaMin = 0.2f;
    [SerializeField] float m_AlphaMax = 0.5f;
    [SerializeField] float m_Lerp= 0.5f;

    SpriteRenderer m_Sprite;
    float          m_Alpha;
    // Start is called before the first frame update
    void Awake()
    {
        m_Sprite = GetComponent<SpriteRenderer>();
        m_Alpha = m_Sprite.color.a;
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,128) < 16)
        {
            m_Alpha = Random.Range(m_AlphaMin, m_AlphaMax);
        }

        Color color = m_Sprite.color;
        color.a = Mathf.Lerp(color.a, m_Alpha, m_Lerp);

        m_Sprite.color = color;
        

    }
}
