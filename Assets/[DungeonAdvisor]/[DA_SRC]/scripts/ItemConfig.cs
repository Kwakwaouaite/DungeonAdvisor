using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Dungeon Advisor/ItemConfig", order = 1)]
public class ItemConfig : ScriptableObject
{
    public UIItem[] m_Chests;
    public UIItem[] m_Enemies;
    public UIItem[] m_Traps;
}
