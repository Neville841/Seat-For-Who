using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    GameManager _gm;

    [SerializeField] TextMeshProUGUI baseCashText;

    private void Awake()
    {
        _gm = GameManager.Instance;
    }
    void Start()
    {
        GameManager_OnCashUpdate();
    }

    void Update()
    {

    }  
    private void OnEnable()
    {
        GameManager.OnCashUpdate += GameManager_OnCashUpdate;
    }

    private void OnDisable()
    {
        GameManager.OnCashUpdate -= GameManager_OnCashUpdate;
    }
    private void GameManager_OnCashUpdate()
    {
        baseCashText.text = _gm.baseCash.ToString();
        print("cashUpdate");
    }
}
