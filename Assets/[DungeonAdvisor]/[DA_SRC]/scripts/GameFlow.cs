using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] UIGrid m_Grid;
    [SerializeField] UITimer m_Timer;

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
        {
            GameManager.SetGameFlowState(m_State);
            yield return StartCoroutine(m_State.ToString(), m_State);
        }

        GameManager.SetGameFlowState(m_State);

    }

    //************************************************
    // fsmMENU
    //------------------------------------------------
    //************************************************
    IEnumerator fsmStartPhase(eSTATE state)
    {
        m_WaveDone = 0;

        yield return waitMessage("Schedule of the day\n\n"+m_GameConfig.m_WaveCount+"waves \n\nwill pass through the dungeon");


        m_State = eSTATE.fsmConstruct;
    }

    //************************************************
    // fsmMENU
    //------------------------------------------------
    //************************************************
    IEnumerator fsmConstruct(eSTATE state)
    {
        m_WaveDone = 0;

        yield return waitMessage("You have \n\n"+ m_GameConfig.m_ConstructDuration + " seconds\n\n to prepare the room");


        m_Timer.SetTimer(m_GameConfig.m_ConstructDuration);

        yield return new WaitForSeconds(m_GameConfig.m_ConstructDuration);


        m_State = eSTATE.fsmWaveReady;
    }

    IEnumerator fsmWaitNextWave(eSTATE state)
    {
        yield return waitMessage("You have \n\n"+ m_GameConfig.m_InterWaveDuration + " seconds\n\n before the next wave\n\nRepair Now");

        m_Timer.SetTimer(m_GameConfig.m_InterWaveDuration);


        yield return new WaitForSeconds(m_GameConfig.m_InterWaveDuration);


        m_State = eSTATE.fsmWaveReady;
    }


    IEnumerator fsmWaveReady(eSTATE state)
    {
        yield return waitMessage("The wave " + (m_WaveDone + 1) + " is ready");

        m_State = eSTATE.fsmWaveRun;

    }

    IEnumerator fsmWaveRun(eSTATE state)
    {
        WaveConfig wave = new WaveConfig();
        wave.InitRandom();

        m_Grid.LaunchExploreRoom(wave);

        yield return null;

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

        yield return waitMessage("The wave "+ (m_WaveDone+1)+" have finished");

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

        yield return waitMessage("The day is Over");
        yield return new WaitForSeconds(1.0f);

        m_State = eSTATE.fsmExit;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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