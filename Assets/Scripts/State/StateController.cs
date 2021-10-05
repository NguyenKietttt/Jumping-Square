using System;
using UnityEngine;

public class StateController : MonoBehaviour
{
    public static event Action OnTitleEvent;
    public static event Action OnTitleToGameplayEvent;
    public static event Action OnGameplayEvent;
    public static event Action OnGameoverEvent;


    public static void RaiseTitleEvent() => OnTitleEvent?.Invoke();

    public static void RaiseTitleToGameplayEvent() => OnTitleToGameplayEvent?.Invoke();

    public static void RaiseGameplayEvent() => OnGameplayEvent?.Invoke();

    public static void RaiseGameoverEvent() => OnGameoverEvent?.Invoke();
}
