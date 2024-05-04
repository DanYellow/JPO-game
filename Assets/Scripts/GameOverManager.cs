using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject mainMenuUI;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;

    private void OnEnable()
    {
        onGameOver.OnEventRaised += DisplayScreen;
    }

    private void DisplayScreen()
    {
        gameOverUI.SetActive(true);
    }

    private void Awake()
    {
        gameOverUI.SetActive(false);
    }

    public void GoBackToIndex()
    {
        gameOverUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= DisplayScreen;
    }
}
