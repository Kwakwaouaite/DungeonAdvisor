using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuToCredits : MonoBehaviour
{
    public GameObject m_Menu;
    public GameObject m_Credits;


    private void OnMouseDown()
    {
        m_Menu.SetActive(false);
        m_Credits.SetActive(true);
    }
}
