using UnityEngine;

public class WaveConfig
{
    public UIItem.eType[] m_Goal;
    public bool[] m_GoalAccomplished;

    public int m_Reward;

    public RoomConfig.eDir m_Door;

    void InitRandom(int numberOfGoals = 3, int minGold = 40, int maxGold = 100)
    {
        m_Goal = new UIItem.eType[numberOfGoals];
        m_GoalAccomplished = new bool[numberOfGoals];
        for (int i=0; i < numberOfGoals; i++)
        {
            m_Goal[i] = (UIItem.eType) Random.Range(0, numberOfGoals);
            m_GoalAccomplished[i] = false;
        }

        int m_Rewards = Random.Range(minGold, maxGold+1);
    }

}