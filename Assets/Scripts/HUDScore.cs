using TMPro;
using UnityEngine;
using System.Globalization;

public class HUDScore : MonoBehaviour
{
    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    private NumberFormatInfo nfi;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;

    private void Start() {
        nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = " ";
    }

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
        string scoreTextFormatted = Mathf.Round(distanceTravelled.CurrentValue).ToString("#,0", nfi);
        scoreText.SetText($"{scoreTextFormatted} m");
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= HideScore;
    }
}
