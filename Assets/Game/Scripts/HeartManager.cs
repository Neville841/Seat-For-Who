using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    [SerializeField] Heart[] hearts;
    [SerializeField] internal int heartIndex;
    private void Start()
    {
        heartIndex = hearts.Length - 1;
    }
    private void OnEnable()
    {
        EventManager.wrongSeat += WrongSeat;
    }
    private void OnDisable()
    {
        EventManager.wrongSeat -= WrongSeat;
    }

    void WrongSeat()
    {
        if (hearts.Length < 0) return;
        if (hearts[heartIndex] != null)
            hearts[heartIndex].Broke();
        else Debug.Log(hearts[heartIndex] + "null");
        heartIndex--;
        if (heartIndex < 0)
            EventManager.OnLevelLose();
    }
}