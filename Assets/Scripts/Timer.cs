using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int timeRemaining = 60;
    [SerializeField]
    private TextMeshProUGUI timerText;

    [Header("Scriptable Objects"), SerializeField]
    private VoidEventChannel onGameEndEvent;

    private bool isGameFinished = false;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    private void OnEnable()
    {
        onGameEndEvent.OnEventRaised += OnGameEnd;
    }

    private void OnGameEnd()
    {
        isGameFinished = true;
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0 && isGameFinished == false)
        {
            timerText.SetText(timeRemaining.ToString());
            timeRemaining--;

            yield return Helpers.GetWait(1);
        }
    }

    private void OnDisable()
    {
        onGameEndEvent.OnEventRaised -= OnGameEnd;
    }
}
