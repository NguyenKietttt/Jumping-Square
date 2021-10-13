using System;
using UnityEngine;

public class StateController : MonoBehaviour
{
    /// <summary>
    /// Raise by RestartButton in Hierarchy
    /// </summary>
    public static void RaiseTitleState() => EventDispatcher.PostEvent(EventsID.TITLE_STATE);

    public static void RaiseTitleToGameplayState() => EventDispatcher.PostEvent(EventsID.TITLE_TO_GAMEPLAY_STATE);

    public static void RaiseGameplayEvent() => EventDispatcher.PostEvent(EventsID.GAMEPLAY_STATE);

    public static void RaiseOnGameplayToGameoverEvent() => EventDispatcher.PostEvent(EventsID.GAMEPLAY_TO_GAMEOVER_STATE);

    public static void RaiseGameoverEvent() => EventDispatcher.PostEvent(EventsID.GAMEOVER_STATE);
}
