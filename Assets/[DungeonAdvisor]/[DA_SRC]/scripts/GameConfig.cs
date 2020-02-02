using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Dungeon Advisor/GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public float m_ConstructDuration = 5.0f;
    public float m_InterWaveDuration = 2.0f;
     
    public float m_WaveSpeed   = 1.0f;
    public int   m_WaveCount   = 3;
}
