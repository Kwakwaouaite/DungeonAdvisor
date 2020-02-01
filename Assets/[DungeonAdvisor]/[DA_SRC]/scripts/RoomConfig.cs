using UnityEngine;

[CreateAssetMenu(fileName = "RoomConfig", menuName = "Dungeon Advisor/RoomConfig", order = 1)]
public class RoomConfig : ScriptableObject
{
    public float m_Column;
    public float m_Row;

    public float m_CellW;
    public float m_CellH;

    public Sprite m_Background;
    public Sprite[] m_BrokenSet;

    public float getWidth() { return m_CellW * m_Column; }
    public float getHeight() { return m_CellH * m_Row;   }
    public int   getCellCount() { return (int)m_Column * (int)m_Row;  }

    public Vector3 getPos(int x, int y)
    {
        Vector3 pos = Vector3.zero;

        pos.x = x * m_CellW;
        pos.y = y * m_CellH;

        return pos;
    }

}
