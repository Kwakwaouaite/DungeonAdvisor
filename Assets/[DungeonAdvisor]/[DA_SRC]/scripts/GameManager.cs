public class GameManager 
{
    static UICell m_ActiveCell;


    static bool m_GroupExploring;


    public static UICell GetActiveCell() { return m_ActiveCell;  }
    public static void   SetActiveCell(UICell cell) { m_ActiveCell = cell;  }
    public static void   ResetActiveCell(UICell cell) {  if (m_ActiveCell == cell) { m_ActiveCell = null; } }

    public static bool IsGroupExploring() { return m_GroupExploring;  }
    public static void StartGroupExploring() { m_GroupExploring = true;  }
    public static void StopGroupExploring() { m_GroupExploring = false; }

    public static bool CanAddItems()
    {
        return !IsGroupExploring();
    }
}
