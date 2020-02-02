﻿using UnityEngine;
using System.Collections;


public class UITopMessage : MonoBehaviour
{
    [SerializeField] Animator m_Anim;
    [SerializeField] TMPro.TextMeshPro m_Text;

    bool m_Open = false;

    // Start is called before the first frame update
    void Start()
    {
        m_Text.SetText("");
    }

    public bool IsOpen()
    {
        return m_Open;
    }

    public void ShowMessage(string text, float duration = 2f)
    {
        m_Open = true;  
        m_Text.SetText(text);

        StartCoroutine(doShow(duration));
    }

    IEnumerator doShow(float duration)
    {

    

        m_Anim.SetTrigger("Show");

        yield return new WaitForSeconds(duration + 1.0f);

        m_Anim.SetTrigger("Hide");


        yield return new WaitForSeconds(1.0f);

        m_Open = false;
    }
   
}
