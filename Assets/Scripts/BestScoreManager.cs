using System.Globalization;
using UnityEngine;
using TMPro;

public class BestScoreManager : MonoBehaviour
{
    [SerializeField]
    private GameObject bestScoreUI;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;

    [SerializeField]
    private FloatValue distanceTravelled;

    private void OnEnable()
    {
        onGameOver.OnEventRaised += DisplayRecord;
    }

    private void DisplayRecord()
    {
        bool hasBrokenPrevRecord = (
            !PlayerPrefs.HasKey("best_score") ||
            PlayerPrefs.HasKey("best_score") && distanceTravelled.CurrentValue > PlayerPrefs.GetFloat("best_score")
        );

        var nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = " ";

        string currentRecordFormatted = Mathf.Round(PlayerPrefs.GetFloat("best_score")).ToString("#,0", nfi);

        TextMeshProUGUI recordText = bestScoreUI.GetComponent<TextMeshProUGUI>();
        if (hasBrokenPrevRecord)
        {
            PlayerPrefs.SetFloat("best_score", distanceTravelled.CurrentValue);
            
            string newRecordFormatted = Mathf.Round(distanceTravelled.CurrentValue).ToString("#,0", nfi);
            recordText.SetText($"Nouveau record !\n{currentRecordFormatted} m → {newRecordFormatted} m");
        }
        else
        {
            recordText.SetText(@$"Record actuel : {currentRecordFormatted} m");
        }
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= DisplayRecord;
    }
}
