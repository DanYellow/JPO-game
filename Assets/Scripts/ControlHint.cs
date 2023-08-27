using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ControlHint : MonoBehaviour
{
    private bool isPlayerInRange = false;

    private new Light2D light;

    [SerializeField]
    private VoidEventChannel onPlayerStartInteractEvent;

    [SerializeField]
    private BoolEventChannel onInteractRangeEvent;

    [SerializeField]
    private VoidEventChannel onPlayerInteractingEvent;

    [SerializeField]
    private StringEventChannel onInteract;
    
    [SerializeField, TextArea]
    private string text;

    private void Awake()
    {
        light = GetComponentInChildren<Light2D>(true);
        light.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        onPlayerStartInteractEvent.OnEventRaised += Display;
    }

    private void Display()
    {
        if (isPlayerInRange)
        {
            onInteract.Raise(text);
            onPlayerInteractingEvent.Raise();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            light.gameObject.SetActive(isPlayerInRange);
            onInteractRangeEvent.Raise(isPlayerInRange);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            light.gameObject.SetActive(false);
            onInteractRangeEvent.Raise(isPlayerInRange);
        }
    }

    private void OnDisable()
    {
        onPlayerStartInteractEvent.OnEventRaised -= Display;
    }

    // public void OnSpeak(InputAction.CallbackContext ctx)
    // {
    //     if (ctx.phase == InputActionPhase.Performed && isPlayerInRange)
    //     {
    //         onSpeakEvent.Raise();
    //     }
    // }
}
