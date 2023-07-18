using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;

    [SerializeField]
    private VoidEventChannel onPlayerDeath;

    private UnityAction onCreditsOrDeathEvent;

    private void Awake()
    {
        onCreditsOrDeathEvent = () => { SetUIGameOverActionMap(); };
        onTogglePauseEvent.OnEventRaised += ToggleActionMap;
        onPlayerDeath.OnEventRaised += onCreditsOrDeathEvent;
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

    private void SetUIGameOverActionMap() {
        playerInput.SwitchCurrentActionMap("UIGameOverAndCredits");
    }

    private void OnDisable()
    {
        onTogglePauseEvent.OnEventRaised -= ToggleActionMap;
        onPlayerDeath.OnEventRaised -= onCreditsOrDeathEvent;
    }
}
