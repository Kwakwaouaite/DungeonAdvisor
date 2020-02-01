using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public enum eSTATE
    {
        fsmCONSTRUCT,
        fsmWAVE,
        fsmISNEXTWAVE,
        fsmExit,

        fsmCOUNT
    };

    public eSTATE m_State;
    public int m_WaveMax = 1;
    public int m_WaveDone = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FSM(eSTATE.fsmCONSTRUCT));
    }

    //************************************************
    // fsmWaiting
    //------------------------------------------------
    //************************************************
    IEnumerator FSM(eSTATE state)
    {
        m_State = state;
        // Execute the current coroutine (state)
        while (m_State != eSTATE.fsmExit)
            yield return StartCoroutine(m_State.ToString(), m_State);
    }


    //************************************************
    // fsmMENU
    //------------------------------------------------
    //************************************************
    IEnumerator fsmCONSTRUCT(eSTATE state)
    {
        Debug.Log("fsmCONSTRUCT");
        m_WaveDone = 0;

        while (state == m_State)
        {

            if (true)
            {

                m_State = eSTATE.fsmWAVE;

            }

            yield return new WaitForSeconds(1);
        }

    }

    IEnumerator fsmWAVE(eSTATE state)
    {
        while (state == m_State)
        {

            if (true)
            {

                m_State = eSTATE.fsmISNEXTWAVE;

            }

            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator fsmISNEXTWAVE(eSTATE state)
    {
        m_WaveDone += 1;

        if (m_WaveMax <= m_WaveDone)
        {
            m_State = eSTATE.fsmCONSTRUCT;
        }
        else
        {
            m_State = eSTATE.fsmWAVE;
        }

        yield return new WaitForSeconds(1);
        
    }

}