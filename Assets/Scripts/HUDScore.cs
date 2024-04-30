using TMPro;
using UnityEngine;

public class HUDScore : MonoBehaviour
{
    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private TextMeshProUGUI scoreText;

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
}
