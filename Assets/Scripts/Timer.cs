using System.Collections;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField]
    private int timeRemaining = 60;
    [SerializeField]
    private TextMeshProUGUI timerText;

    private void Start()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        while (timeRemaining > 0)
        {
            timerText.SetText(timeRemaining.ToString());
            timeRemaining--;

            yield return Helpers.GetWait(1);
        }
    }
}
