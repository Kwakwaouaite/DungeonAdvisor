using UnityEngine;

[CreateAssetMenu(fileName = "ScreenRefConfig", menuName = "Dungeon Advisor/ScreenRefConfig", order = 1)]
public class ScreenRefConfig : ScriptableObject
{
    [SerializeField] float m_Width;
    [SerializeField] float m_Height;

    public float getRatio() { return m_Width / m_Height; }
    public float getWidth() { return m_Width / (getRatio() * 2.0f);  }
    public float getHeight() { return m_Height / 2.0f;  }
}
