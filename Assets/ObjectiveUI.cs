using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveUI : MonoBehaviour
{
    public WaveConfig waveConfig;
    public TMPro.TextMeshPro textMesh;

    private void Awake()
    {
        waveConfig = null;
    }

    // Update is called once per frame
    void Update()
    {
        if (waveConfig != null)
        {
            textMesh.text = "Arrive: " + waveConfig.m_Door.ToString().ToLower();
            textMesh.text += "\nObjective: ";

            for (int i = 0; i< waveConfig.m_Goal.Length; i++)
            {
                if (waveConfig.m_Goal[i] == UIItem.eType.Enemy)
                {
                    textMesh.text += "\n- scorpion";
                }
                else
                {
                    textMesh.text += "\n- " + waveConfig.m_Goal[i].ToString().ToLower();
                }
            }

            textMesh.text += "\nReward: " + waveConfig.m_Reward;
        }
    }
}
