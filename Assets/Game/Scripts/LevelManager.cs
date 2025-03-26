using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameObject winPanel;
    [SerializeField] GameObject losePanel;

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
        winPanel.SetActive(true);
    }
    void LevelLose()
    {
        losePanel.SetActive(true);
    }
}
