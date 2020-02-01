using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager 
{
    static UICell m_ActiveCell;

    public static UICell GetActiveCell() { return m_ActiveCell;  }
    public static void   SetActiveCell(UICell cell) { m_ActiveCell = cell;  }
    public static void   ResetActiveCell(UICell cell) {  if (m_ActiveCell == cell) { m_ActiveCell = null; } }
}
