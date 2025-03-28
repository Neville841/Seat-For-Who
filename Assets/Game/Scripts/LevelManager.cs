using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class LevelManager : MonoBehaviour
{
    [Inject] AudioManager audioManager;
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] GameObject gamePanel;
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;
    [SerializeField] ParticleSystem winVfx;
    private void Start()
    {
        levelText.text = "LEVEL:" + PlayerPrefs.GetInt("_level", 1).ToString();
    }
    private void OnEnable()
    {
        EventManager.levelWin += LevelWin;
        EventManager.levelLose += LevelLose;
    }
    private void OnDisable()
    {
        EventManager.levelWin -= LevelWin;
        EventManager.levelLose -= LevelLose;
    }
    void LevelWin()
    {
        PlayerPrefs.SetInt("_level", PlayerPrefs.GetInt("_level", 1) + 1);
        if (PlayerPrefs.GetInt("_level", 1) == 4)
        {
            PlayerPrefs.SetInt("_level", 1);
        }
        winVfx.Simulate(0, true, true);
        winVfx.Play();
        audioManager.Play("Confetti");
        Taptic.Heavy();
        Invoke("WinPanelDelay", 2);
    }
    void WinPanelDelay()
    {
        audioManager.Play("Win");
        gamePanel.SetActive(false);
        winPanel.SetActive(true);
    }
    void LevelLose()
    {
        Taptic.Medium();
        audioManager.Play("Fail");
        losePanel.SetActive(true);
    }
    public void LevelChange()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("_level", 1));
    }
}
