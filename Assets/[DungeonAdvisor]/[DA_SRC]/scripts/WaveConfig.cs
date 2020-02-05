using UnityEngine;

public class WaveConfig
{
    public enum eState
    {
        Waiting,
        Running,
        Finished
    };

    public eState m_State; 
    public int m_WaveID;
    public UIItem.eType[] m_Goal;
    public bool[] m_GoalAccomplished;

    public int m_Reward;

    public RoomConfig.eDir m_Door;

    public void InitRandom(int waveID,int numberOfGoals = 3, int minGold = 60, int maxGold = 200)
    {
        m_WaveID = waveID;
        m_State = eState.Waiting;
        m_Goal = new UIItem.eType[numberOfGoals];
        m_GoalAccomplished = new bool[numberOfGoals];
        for (int i=0; i < numberOfGoals; i++)
        {
            m_Goal[i] = (UIItem.eType) Random.Range(0, (int) UIItem.eType.Count);
            if (m_Goal[i] == UIItem.eType.Door || m_Goal[i] == UIItem.eType.Torch)
            {
                m_Goal[i] = UIItem.eType.Chest;
            }
            m_GoalAccomplished[i] = false;
        }

        m_Reward = Random.Range(minGold, maxGold+1);

        m_Door  = (RoomConfig.eDir) Random.Range(0, (int) RoomConfig.eDir.COUNT);
    }

}