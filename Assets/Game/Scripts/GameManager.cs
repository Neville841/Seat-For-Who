using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : Singleton<GameManager>
{
    [SerializeField] internal float baseCash;
    public static event Action OnCashUpdate;

    private void Awake()
    {
        baseCash = PlayerPrefs.GetFloat("_baseCash", 0);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void UpdateCash(float cashValue)
    {
        baseCash += cashValue;
        PlayerPrefs.SetFloat("_baseCash", baseCash);
        OnCashUpdate?.Invoke();

    }
    public void Payment(float cashValue)
    {
        baseCash -= cashValue;
        PlayerPrefs.SetFloat("_baseCash", baseCash);
        OnCashUpdate?.Invoke();
    }
    public void CashTest(bool payment)
    {
        if (!payment) UpdateCash(10);
        if (payment && baseCash > 0) Payment(10);
    }
}
