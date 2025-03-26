using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartManager : MonoBehaviour
{
    [SerializeField] Heart[] hearts;
    int heartIndex;
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
        hearts[heartIndex].Broke();
        heartIndex--;
    }
}