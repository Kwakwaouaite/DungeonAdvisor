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
    [SerializeField] ObjectiveUI WaveUI1;
    [SerializeField] ObjectiveUI WaveUI2;
    [SerializeField] ObjectiveUI WaveUI3;

    private WaveConfig[] m_Waves;
    [SerializeField] AudioSource m_BGMFight;
    [SerializeField] AudioSource m_BGMAmbient;

    [SerializeField] float m_MessageVolume = 0.5f;

    public GameConfig m_GameConfig;
    public eSTATE m_State;

    public float m_BGMFightVol = 0.0f;
    public float m_BGMFightDestVol = 0.0f;


    public float m_BGMAmbientVol = 0.0f;
    public float m_BGMAmbientDestVol = 0.0f;

    public int m_WaveDone = 0;

    public void Start()
    {
        StartGameFlow();

        StartCoroutine(playBGMFight());
        StartCoroutine(playBGMAmbient());

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
    // 
    //------------------------------------------------
    //************************************************
    IEnumerator playBGMFight()
    {
        m_BGMFightVol = 0.0f;
        while (true)
        {
            m_BGMFightVol = Mathf.Lerp(m_BGMFightVol, m_BGMFightDestVol, 0.1f);
            m_BGMFight.volume = m_BGMFightVol;
            yield return null;
        }
    }

    IEnumerator playBGMAmbient()
    {
        m_BGMAmbientVol = 0.0f;
        while (true)
        {
            m_BGMAmbientVol = Mathf.Lerp(m_BGMAmbientVol, m_BGMAmbientDestVol, 0.1f);
            m_BGMAmbient.volume = m_BGMAmbientVol;
            yield return null;
        }
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
        m_BGMFightDestVol = 0f;
        m_BGMAmbientDestVol = m_MessageVolume;

        m_Waves = new WaveConfig[3];

        for (int i = 0; i < 3; i++)
        {
            m_Waves[i] = new WaveConfig();
            m_Waves[i].InitRandom();
        }

        WaveUI1.waveConfig = m_Waves[0];
        WaveUI2.waveConfig = m_Waves[1];
        WaveUI3.waveConfig = m_Waves[2];

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

        m_BGMFightDestVol = 0f;
        m_BGMAmbientDestVol = 1.0f;

        m_Timer.SetTimer(m_GameConfig.m_ConstructDuration);

        yield return new WaitForSeconds(m_GameConfig.m_ConstructDuration);


        m_State = eSTATE.fsmWaveReady;
    }

    IEnumerator fsmWaitNextWave(eSTATE state)
    {


        yield return waitMessage("You have \n\n"+ m_GameConfig.m_InterWaveDuration + " seconds\n\n before the next wave\n\nRepair Now");

        m_Timer.SetTimer(m_GameConfig.m_InterWaveDuration);

        m_BGMFightDestVol = 0f;
        m_BGMAmbientDestVol = 1.0f;


        yield return new WaitForSeconds(m_GameConfig.m_InterWaveDuration);


        m_State = eSTATE.fsmWaveReady;
    }


    IEnumerator fsmWaveReady(eSTATE state)
    {

        m_BGMFightDestVol = 0f;
        m_BGMAmbientDestVol = m_MessageVolume;

        yield return waitMessage("The wave " + (m_WaveDone + 1) + " is ready");

        m_State = eSTATE.fsmWaveRun;

    }

    IEnumerator fsmWaveRun(eSTATE state)
    {
        m_BGMFightDestVol = 1.0f;
        m_BGMAmbientDestVol = 0.0f;

        GameManager.m_GoldReward = 0;
        GameManager.m_Happiness = 0.0f;


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

        m_BGMFightDestVol = 0.0f;
        m_BGMAmbientDestVol = m_MessageVolume;

    }

    IEnumerator fsmWaveFinished(eSTATE state)
    {
        int percent = (int)Mathf.Clamp(Mathf.Ceil(GameManager.m_Happiness * 100),0,100);
        yield return waitMessage("The wave "+ (m_WaveDone+1)+" have finished\n\n"
            +"They are "+percent+" % happy \n\n" 
            +"You win \n"+GameManager.m_GoldReward+"golds\n\n");

        GameManager.AddGold(GameManager.m_GoldReward);
        GameManager.m_GoldReward = 0;
        GameManager.m_Happiness = 0.0f;

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