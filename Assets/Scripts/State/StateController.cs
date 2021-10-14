using System;
using UnityEngine;

public class StateController : MonoBehaviour
{
    /// <summary>
    /// Raise by RestartButton in Hierarchy
    /// </summary>
    public static void RaiseTitleState()
    {
        CustomLogs.Instance.Log("<color=green> Start" + EventsID.TITLE_STATE + "</color>");
        EventDispatcher.PostEvent(EventsID.TITLE_STATE);
    }

    public static void RaiseTitleToGameplayState()
    {
        CustomLogs.Instance.Log("<color=green> End" + EventsID.TITLE_STATE + "</color>");
        
        CustomLogs.Instance.Log("<color=blue>" + EventsID.TITLE_TO_GAMEPLAY_STATE + "</color>");
        EventDispatcher.PostEvent(EventsID.TITLE_TO_GAMEPLAY_STATE);
    }

    public static void RaiseGameplayEvent()
    {
        CustomLogs.Instance.Log("<color=red>" + EventsID.GAMEPLAY_STATE + "</color>");
        EventDispatcher.PostEvent(EventsID.GAMEPLAY_STATE);
    }

    public static void RaiseOnGameplayToGameoverEvent()
    {
        CustomLogs.Instance.Log("<color=yellow>" + EventsID.GAMEPLAY_TO_GAMEOVER_STATE + "</color>");
        EventDispatcher.PostEvent(EventsID.GAMEPLAY_TO_GAMEOVER_STATE);
    }

    public static void RaiseGameoverEvent()
    {
        CustomLogs.Instance.Log("<color=white>" + EventsID.GAMEOVER_STATE + "</color>");
        EventDispatcher.PostEvent(EventsID.GAMEOVER_STATE);
    }
}
