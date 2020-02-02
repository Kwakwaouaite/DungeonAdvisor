using UnityEngine;

public class SpriteFX_AdditiveFlash : MonoBehaviour
{

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
            m_Alpha = Random.Range(0.2f, 0.5f);
        }

        Color color = m_Sprite.color;
        color.a = Mathf.Lerp(color.a, m_Alpha, 0.1f);

        m_Sprite.color = color;
        

    }
}
