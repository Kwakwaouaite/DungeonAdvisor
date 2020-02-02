using UnityEngine;
using System.Collections.Generic;

public static class ActionHistory
{
    public enum eACTION
    {
        Damage,
        Repair,
        AddItem,
    };

    public class Action
    {
        public eACTION m_Action;
        public UICell m_Cell;
        public UIItem m_Item;
    }

    static Stack<Action> m_History      = new Stack<Action>();
    static Stack<Action> m_HistoryRedo  = new Stack<Action>();

    public static void Reset()
    {
        m_History.Clear();
        m_HistoryRedo.Clear();
    }

    public static void Do(eACTION actionType, UICell cell, UIItem item = null)
    {
        m_HistoryRedo.Clear();

        Action action = new Action();
        action.m_Action = actionType;
        action.m_Cell = cell;
        action.m_Item = item;

        m_History.Push(action);
    }

    public static void Undo()
    {
        if (m_History.Count == 0)
        {
            return;
        }
        Action action = m_History.Pop();

        switch (action.m_Action)
        {
            case eACTION.Damage:
                {
                    action.m_Cell.DoRepair();
                }
                break;

            case eACTION.Repair:
                {
                    action.m_Cell.DoDamage();
                }
                break;
            case eACTION.AddItem:
                {
               //     action.m_Cell.RemoveItem();
                }
                break;
        }

        m_HistoryRedo.Push(action);
    }

    public static void Redo()
    {
        if (m_HistoryRedo.Count == 0)
        {
            return;
        }

        Action action = m_History.Pop();
        switch (action.m_Action)
        {
            case eACTION.Damage:
                {
                    action.m_Cell.DoRepair();
                }

                break;

            case eACTION.Repair:
                {
                    action.m_Cell.DoDamage();
                }
                break;

            case eACTION.AddItem:
                {
                    action.m_Cell.AddItem(action.m_Item);
                }

                break;
        }
        m_History.Push(action);
    }


}
