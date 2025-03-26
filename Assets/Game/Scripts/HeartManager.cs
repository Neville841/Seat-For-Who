using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    [SerializeField] Heart[] hearts;
    internal int heartIndex;
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
        hearts[heartIndex].Broke();
        heartIndex--;
        if (heartIndex < 0)
            EventManager.OnLevelLose();
    }
}