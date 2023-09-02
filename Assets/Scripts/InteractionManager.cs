using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionManager : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannel onInteractRangeEvent;

    [SerializeField]
    private StringEventChannel onInteractingEvent;


    [SerializeField]
    public GameObject interactionUI;

    private GameObject continueText;

    private TMP_Text textContainer;

    [SerializeField, TextArea]
    private string initialString;

    private int nbCalls = 0;

    private void Awake() {
        textContainer = interactionUI.GetComponentInChildren<TMP_Text>();
        continueText = interactionUI.transform.Find("Container/Background/Continue").gameObject;

        continueText.SetActive(false);
        interactionUI.SetActive(false);
    }

    private void OnEnable()
    {
        onInteractRangeEvent.OnEventRaised += ToggleDisplay;
        onInteractingEvent.OnEventRaised += UpdateText;
    }

    private void ToggleDisplay(bool show)
    {
        textContainer.SetText(initialString);
        interactionUI.SetActive(show);
    }

    private void UpdateText(string text) {
        textContainer.SetText(text);
    }

    private void OnDisable()
    {
        onInteractRangeEvent.OnEventRaised -= ToggleDisplay;
        onInteractingEvent.OnEventRaised -= UpdateText;
    }
}
