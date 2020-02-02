using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditToMenu : MonoBehaviour
{
    public GameObject m_Menu;


    private void OnMouseDown()
    {
        m_Menu.SetActive(true);
        this.gameObject.SetActive(false);
    }
}
