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
            textMesh.text = "Arrive: " + waveConfig.m_Door.ToString();
            textMesh.text += "\nObjective: ";

            for (int i = 0; i< waveConfig.m_Goal.Length; i++)
            {

            }
        }
    }
}
