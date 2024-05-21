using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class HowToPlayManager : MonoBehaviour
{
    [SerializeField]
    private GameObject howToPlayUI;

    [SerializeField]
    private TextMeshProUGUI instructionsText;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onDisplayHowToPlayScreen;

    [SerializeField]
    private VoidEventChannel onHideHowToPlayScreen;

    private void Awake()
    {
        howToPlayUI.SetActive(false);
    }

    private void OnEnable()
    {
        onDisplayHowToPlayScreen.OnEventRaised += DisplayScreen;
    }

    private void UpdateResult()
    {
        NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.InvariantCulture.NumberFormat.Clone();
        nfi.NumberGroupSeparator = " ";

        float currentRecord = PlayerPrefs.HasKey("best_score") ? PlayerPrefs.GetFloat("best_score") : 0;

        string userDistance = Regex.Match(instructionsText.text, "<color=#AAAAFF>(.*?)</color>").Groups[1].ToString();
        string userDistanceTagColor = Regex.Match(userDistance, "<color=#([A-z0-9]*)>").Groups[0].ToString();
        string distanceTravelledFormatted = Mathf.Round(currentRecord).ToString("#,0", nfi);

        string currentRecordTextComputed = $"{userDistanceTagColor}{distanceTravelledFormatted} m";

        string instructionsTextComputed = instructionsText.text.Replace(userDistance, currentRecordTextComputed);

        instructionsText.SetText(instructionsTextComputed);
    }

    private void DisplayScreen()
    {
        UpdateResult();
        howToPlayUI.SetActive(true);
        ExtensionsEventSystem.UpdateSelectedGameObject(howToPlayUI.GetComponentInChildren<Button>().gameObject);
    }

    public void GoBack()
    {
        howToPlayUI.SetActive(false);
        onHideHowToPlayScreen.OnEventRaised();
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void OnDisable()
    {
        onDisplayHowToPlayScreen.OnEventRaised -= DisplayScreen;
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (howToPlayUI != null && howToPlayUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            howToPlayUI.GetComponentInChildren<Button>().Select();
        }
    }
}
