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
    private VoidEventChannel onStartGame;

    private void Awake()
    {
        scoreText.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        onStartGame.OnEventRaised += DisplayCounter;
    }

    private void DisplayCounter()
    {
        scoreText.gameObject.SetActive(true);
    }

    void Update()
    {
        string _scoreText = $"{Mathf.Round(distanceTravelled.CurrentValue)} m";
        scoreText.SetText(_scoreText);
    }

    private void OnDisable()
    {
        onStartGame.OnEventRaised -= DisplayCounter;
    }
}
