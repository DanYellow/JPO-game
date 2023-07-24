using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class ActionMapName
{
    public static string Player = "Player";
    public static string UI = "UI";
    public static string UIGameOverAndCredits = "UIGameOverAndCredits";
    public static string Cinematics = "Cinematics";
}

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;

    [SerializeField]
    private VoidEventChannel onPlayerDeath;
    
    [SerializeField]
    private VoidEventChannel onCinematicStartEvent;

    private UnityAction onCreditsOrDeathEvent;
    private UnityAction onCinematic;

    private void Awake()
    {
        onCreditsOrDeathEvent = () => { SwitchActionMap(ActionMapName.UIGameOverAndCredits); };
        onTogglePauseEvent.OnEventRaised += ToggleActionMap;
        onPlayerDeath.OnEventRaised += onCreditsOrDeathEvent;

        onCinematic = () => { SwitchActionMap(ActionMapName.Cinematics); };
        onCinematicStartEvent.OnEventRaised += onCinematic;
    }

    public void ToggleActionMap(bool isPaused)
    {
        if (isPaused)
        {
            SwitchActionMap(ActionMapName.UI);
        }
        else
        {
            SwitchActionMap(ActionMapName.Player);
        }
    }

    private void SwitchActionMap(string mapName = null)
    {
        mapName = mapName ?? ActionMapName.Player;
        playerInput.SwitchCurrentActionMap(mapName);
    }

    private void OnDisable()
    {
        onTogglePauseEvent.OnEventRaised -= ToggleActionMap;
        onPlayerDeath.OnEventRaised -= onCreditsOrDeathEvent;
        onCinematicStartEvent.OnEventRaised -= onCinematic;
    }
}
