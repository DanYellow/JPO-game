using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControlHint : MonoBehaviour
{
    private bool isPlayerInRange = false;

    [SerializeField]
    private VoidEventChannel onInteractEvent;

     private void OnEnable() {
        onInteractEvent.OnEventRaised += Display;
    }

    private void Display() {
        if(isPlayerInRange) {
            Debug.Log("Hello");
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            isPlayerInRange = false;
        }
    }

    private void OnDisable() {
        onInteractEvent.OnEventRaised -= Display;
    }

    // public void OnSpeak(InputAction.CallbackContext ctx)
    // {
    //     if (ctx.phase == InputActionPhase.Performed && isPlayerInRange)
    //     {
    //         onSpeakEvent.Raise();
    //     }
    // }
}
