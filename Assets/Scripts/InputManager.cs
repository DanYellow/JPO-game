using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;

    [SerializeField]
    private VoidEventChannel onBossKilled;

    private UnityAction onCreditsEvent;

    private void Awake()
    {
        onCreditsEvent = () => { ToggleActionMap(true); };
        onTogglePauseEvent.OnEventRaised += ToggleActionMap;
        onBossKilled.OnEventRaised += onCreditsEvent;
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
        onBossKilled.OnEventRaised -= onCreditsEvent;
    }
}
