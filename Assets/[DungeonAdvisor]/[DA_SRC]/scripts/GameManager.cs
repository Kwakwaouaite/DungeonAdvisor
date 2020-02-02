public class GameManager 
{
    static UICell m_ActiveCell;


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
