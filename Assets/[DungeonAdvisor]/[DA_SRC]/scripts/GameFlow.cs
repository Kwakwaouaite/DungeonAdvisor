﻿using System.Collections;
using UnityEngine;

public class GameFlow : MonoBehaviour
{
    public enum eSTATE
    {
        fsmStartPhase,
        fsmConstruct,
        fsmWaitNextWave,
        fsmWaveReady,
        fsmWaveRun,
        fsmWaveFinished,
        fsmIsNextWave,
        fsmPhaseEnd,
        fsmExit,

        fsmCOUNT
    };

    [SerializeField] UITopMessage m_Message;

    public GameConfig m_GameConfig;
    public eSTATE m_State;

    public int m_WaveDone = 0;

    public void Start()
    {
        StartGameFlow();
    }

    public void StartGameFlow()
    {
        StartCoroutine(FSM(eSTATE.fsmStartPhase));
    }


    public bool IsGameFlowFinished()
    {
        return m_State == eSTATE.fsmExit;
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
    IEnumerator fsmStartPhase(eSTATE state)
    {
        m_WaveDone = 0;

        yield return waitMessage("Get Ready for New Day");


        m_State = eSTATE.fsmConstruct;
    }

    //************************************************
    // fsmMENU
    //------------------------------------------------
    //************************************************
    IEnumerator fsmConstruct(eSTATE state)
    {
        m_WaveDone = 0;

        yield return waitMessage("Construct Now");


        yield return new WaitForSeconds(m_GameConfig.m_ConstructDuration);

        m_State = eSTATE.fsmWaveReady;
    }

    IEnumerator fsmWaitNextWave(eSTATE state)
    {
        yield return waitMessage("Repair Now");


        yield return new WaitForSeconds(m_GameConfig.m_InterWaveDuration);

        m_State = eSTATE.fsmWaveReady;
    }


    IEnumerator fsmWaveReady(eSTATE state)
    {
        yield return waitMessage("The Hero are ready");

        m_State = eSTATE.fsmWaveRun;

    }

    IEnumerator fsmWaveRun(eSTATE state)
    {
        while (state == m_State)
        {
            if (!GameManager.IsGroupExploring())
            {
                m_State = eSTATE.fsmWaveFinished;
            }

            yield return null;
        }
    }

    IEnumerator fsmWaveFinished(eSTATE state)
    {

        yield return waitMessage("The Hero are finished");

        m_State = eSTATE.fsmIsNextWave;
    }

    IEnumerator fsmIsNextWave(eSTATE state)
    {
        m_WaveDone++;
        if (m_WaveDone < m_GameConfig.m_WaveCount)
        {
            m_State = eSTATE.fsmWaitNextWave;
        }
        else
        {
            m_State = eSTATE.fsmPhaseEnd;
        }

        yield return null;
    }


    IEnumerator fsmPhaseEnd(eSTATE state)
    {

        yield return waitMessage("Day is Over");
        yield return new WaitForSeconds(1.0f);

        m_State = eSTATE.fsmExit;
    }

    IEnumerator waitMessage(string text, float duration = 2.0f)
    {
        m_Message.ShowMessage(text,duration);

        while (m_Message.IsOpen())
        {
            yield return null;
        }
    }
}