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

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onDisplayHowToPlayScreen;

    private void Awake()
    {
        howToPlayUI.SetActive(false);
    }

    private void OnEnable()
    {
        onDisplayHowToPlayScreen.OnEventRaised += DisplayScreen;
    }

    private void DisplayScreen()
    {
        howToPlayUI.SetActive(true);
        ExtensionsEventSystem.UpdateSelectedGameObject(howToPlayUI.GetComponentInChildren<Button>().gameObject);
    }

    public void GoBack()
    {
        howToPlayUI.SetActive(false);
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
