using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class EventManager
{
    public static event UnityAction wrongSeat;
    public static void OnWrongSeat() => wrongSeat?.Invoke();

    public static event UnityAction levelWin;
    public static void OnLevelWin() => levelWin?.Invoke();

    public static event UnityAction levelLose;
    public static void OnLevelLose() => levelLose?.Invoke();
}