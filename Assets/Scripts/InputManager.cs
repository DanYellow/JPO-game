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

    [SerializeField]
    private VoidEventChannel onPlayerDeath;

    private UnityAction onCreditsOrDeathEvent;

    private void Awake()
    {
        onCreditsOrDeathEvent = () => { SetUIGameOverActionMap(); };
        onTogglePauseEvent.OnEventRaised += ToggleActionMap;
        onBossKilled.OnEventRaised += onCreditsOrDeathEvent;
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

    private void OnDestroy()
    {
        onTogglePauseEvent.OnEventRaised -= ToggleActionMap;
        onBossKilled.OnEventRaised -= onCreditsOrDeathEvent;
        onPlayerDeath.OnEventRaised -= onCreditsOrDeathEvent;
    }

    private void OnEnable()
    {
        // playerInput.enabled = true;
    }

    // private void OnDisable()
    // {
    //     playerInput.enabled = false;
    // }
}
