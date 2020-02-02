public class GameManager 
{
    static public SoundManager SoundManager;
    static UICell m_ActiveCell;

    static int m_Gold = 400;
    static bool m_GroupExploring;
    static bool m_PopupMessageOpen;

    static GameFlow.eSTATE m_GameFlowSate;

    static public GameFlow.eSTATE GetFlowState() { return m_GameFlowSate;  }
    static public void            SetGameFlowState(GameFlow.eSTATE state) { m_GameFlowSate = state;  }

    public static UICell GetActiveCell() { return m_ActiveCell;  }
    public static void   SetActiveCell(UICell cell) { m_ActiveCell = cell;  }
    public static void   ResetActiveCell(UICell cell) {  if (m_ActiveCell == cell) { m_ActiveCell = null; } }

    public static bool IsGroupExploring() { return m_GroupExploring;  }
    public static void StartGroupExploring() { m_GroupExploring = true;  }
    public static void StopGroupExploring() { m_GroupExploring = false; }

    public static bool IsPopupMessageOpen() { return m_PopupMessageOpen; }
    public static void PopupMessageOpen() { m_PopupMessageOpen = true; }
    public static void PopupMessageClosed() { m_PopupMessageOpen = false; }

    public static bool HasEnoughGold(int value) { return value <= m_Gold; }
    public static int GetGold() { return m_Gold; }
    public static void AddGold(int value) { m_Gold += value; }
    public static void DecGold(int value)
    {
        m_Gold -= value;
        if (m_Gold < 0)
        {
            m_Gold = 0;
        }

        if (SoundManager)
        {
            SoundManager.PlayCoinSound();
        }
    }

    public static bool CanAddItems()
    {
        return
                (m_GameFlowSate == GameFlow.eSTATE.fsmConstruct)
            && (!IsGroupExploring())
            && (!IsPopupMessageOpen())
            ;
    }

    public static bool canRepair()
    {
        return
              (
              (m_GameFlowSate == GameFlow.eSTATE.fsmConstruct)
           ||   (m_GameFlowSate == GameFlow.eSTATE.fsmWaitNextWave)
           )
           && (!IsGroupExploring())
           && (!IsPopupMessageOpen())
           ;
    }
}
