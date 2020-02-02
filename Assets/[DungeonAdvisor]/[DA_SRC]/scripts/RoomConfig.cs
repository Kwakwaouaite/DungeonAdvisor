using UnityEngine;

[CreateAssetMenu(fileName = "RoomConfig", menuName = "Dungeon Advisor/RoomConfig", order = 1)]
public class RoomConfig : ScriptableObject
{
    public enum eDir
    {
        UP,
        RIGHT,
        DOWN,
        LEFT,

        COUNT
    };


    public float m_Column;
    public float m_Row;

    public float m_CellW;
    public float m_CellH;

    public Sprite[] m_Background;
    public Sprite[] m_BrokenSet;
    public Sprite[] m_LiquidSet;
    public Color[] m_HoverColor;
    public UIItem[] m_Doors ;

    public float getWidth() { return m_CellW * m_Column; }
    public float getHeight() { return m_CellH * m_Row;   }
    public int   getCellCount() { return (int)m_Column * (int)m_Row;  }

    public Sprite getBackround() { return m_Background[Random.Range(0, m_Background.Length)]; }
    public Vector3 getPos(int x, int y)
    {
        Vector3 pos = Vector3.zero;

        pos.x = x * m_CellW;
        pos.y = y * m_CellH;

        return pos;
    }

    public UIItem GetDoor(eDir dir)
    {
        return m_Doors[(int)dir];
    }

    public int GetLeft() { return 0; }
    public int GetRight() { return (int)(m_Column-1); }
    public int GetUp() { return (int)(m_Row-1); }
    public int GetDown() { return 0; }

}
