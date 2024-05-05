using TMPro;
using UnityEngine;

public class HUDScore : MonoBehaviour
{
    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;

    private void OnEnable()
    {
        onGameOver.OnEventRaised += HideScore;
    }

    private void HideScore()
    {
        gameObject.SetActive(false);
    }

    private void Awake()
    {
        scoreText.SetText("");
        gameObject.SetActive(false);
    }

    void Update()
    {
        string _scoreText = $"{Mathf.Round(distanceTravelled.CurrentValue)} m";
        scoreText.SetText(_scoreText);
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= HideScore;
    }
}
