using UnityEngine.InputSystem;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;
    public BoolEventChannel onTogglePauseEvent;

    private void Start() {
        onTogglePauseEvent.OnEventRaised += ToggleActionMap;
    }

    public void ToggleActionMap(bool isPaused)
    {
        if (isPaused)
        {
            playerInput.SwitchCurrentActionMap("UI");
        }
        else
        {
            playerInput.SwitchCurrentActionMap("Player");
        }
    }

    private void OnDestroy()
    {
        onTogglePauseEvent.OnEventRaised -= ToggleActionMap;
    }
}
