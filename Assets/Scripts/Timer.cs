using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int timeRemaining = 60;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [SerializeField]
    private Image background;
    [SerializeField]
    private Image icon;

    [Header("Scriptable Objects"), SerializeField]
    private VoidEventChannel onGameEndEvent;

    [SerializeField]
    private VoidEventChannel onTimerEndEvent;

    [SerializeField]
    private VoidEventChannel onGameStartEvent;

    private bool isGameFinished = false;
    private Color redColor = new Color(0.735849f, 0, 0);

    private void Start()
    {
        if (timeRemaining <= 5)
        {
            background.color = redColor;
            timerText.color = Color.white;
            icon.color = Color.white;
        }
    }

    private void OnEnable()
    {
        onGameEndEvent.OnEventRaised += OnGameEnd;
        onGameStartEvent.OnEventRaised += OnGameStart;
    }

    private void OnGameEnd()
    {
        isGameFinished = true;
    }

    private IEnumerator Countdown()
    {
        timerText.SetText(timeRemaining.ToString());
        while (timeRemaining > -1 && isGameFinished == false)
        {
            yield return Helpers.GetWait(1);
            timerText.SetText(timeRemaining.ToString());
            timeRemaining--;

            if (timeRemaining <= 5)
            {
                background.color = redColor;
                timerText.color = Color.white;
                icon.color = Color.white;
            }
        }

        if (timeRemaining <= 0)
        {
            onTimerEndEvent.Raise();
        }
    }

    private void OnGameStart()
    {
        StartCoroutine(Countdown());
    }

    private void OnDisable()
    {
        onGameEndEvent.OnEventRaised -= OnGameEnd;
        onGameStartEvent.OnEventRaised -= OnGameStart;
    }
}
