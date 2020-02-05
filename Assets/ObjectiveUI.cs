using UnityEngine;

public class ObjectiveUI : MonoBehaviour
{
    public ItemConfig itemConfig;
    public WaveConfig waveConfig;
    public TMPro.TextMeshPro textMesh;
    public TMPro.TextMeshPro title;
    public SpriteRenderer[] m_Reward;
    public SpriteRenderer m_Background;

    public Color m_colReward;
    public Color m_colRewardDone;
    public Color m_colRewardFailed;
    

    public Color m_colBGWaiting;
    public Color m_colBGRunning;
    public Color m_colBGFinished;


    private void Awake()
    {
        waveConfig = null;
    }

    Sprite GetSprite(UIItem.eType item)
    {
        switch(item)
        {
            case UIItem.eType.Chest: return itemConfig.m_Chest;
            case UIItem.eType.Enemy: return itemConfig.m_Scorpion;
            case UIItem.eType.Trap: return itemConfig.m_Trap;
            case UIItem.eType.Princess: return itemConfig.m_Princess;
            case UIItem.eType.Potion: return itemConfig.m_Potion;
            case UIItem.eType.Helmet: return itemConfig.m_Helmet;
            case UIItem.eType.Jar: return itemConfig.m_Jar;
            case UIItem.eType.Gold: return itemConfig.m_Gold;
        }

        return null;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveConfig != null)
        {
            switch(waveConfig.m_State)
            {
                case WaveConfig.eState.Waiting: m_Background.color = m_colBGWaiting; break;
                case WaveConfig.eState.Running: m_Background.color = m_colBGRunning; break;
                case WaveConfig.eState.Finished: m_Background.color = m_colBGFinished; break;
            }
            
                           
            title.text = "<b>Wave:</b> <#FFFFFF><size=1.>"+waveConfig.m_WaveID;
            textMesh.text = "<b>Door:</b> <#FFFFFF>" + waveConfig.m_Door.ToString().ToLower();
            textMesh.text += "\n<#2E8AB9><b>Objective:</b> ";

            for (int i=0; i<3;i++)
            {
                m_Reward[i].gameObject.SetActive(false);
            }

            int reward = 0;
            for (int i = 0; i< waveConfig.m_Goal.Length; i++)
            {
                Sprite spr = GetSprite(waveConfig.m_Goal[i]);
                if (spr==null)
                {
                    continue;
                }

                m_Reward[reward].sprite = spr;
                if (waveConfig.m_GoalAccomplished[i] )
                {
                    m_Reward[reward].color = m_colRewardDone ;
                }
                else
                {
                    m_Reward[reward].color = (waveConfig.m_State == WaveConfig.eState.Finished) ? m_colRewardFailed : m_colReward;
                    
                }
                m_Reward[reward].gameObject.SetActive(true);
                reward++;


            }

            textMesh.text += "\n\n\n<#2E8AB9><b>Reward:</b> <#FFFFFF>" + waveConfig.m_Reward;
        }
    }
}
