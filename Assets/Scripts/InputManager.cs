using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class ActionMapName
{
    public static string Player = "Player";
    public static string UI = "UI";
    public static string UIGameOverAndCredits = "UIGameOverAndCredits";
    public static string Cinematics = "Cinematics";
    public static string Interact = "Interact";
}

public class InputManager : MonoBehaviour
{
    public PlayerInput playerInput;

    [SerializeField]
    private StringEventChannel onPlayerInputMapChange;

    private void Awake()
    {
        // onCreditsOrDeathEvent = () => { SwitchActionMap(ActionMapName.UIGameOverAndCredits); };
        // onTogglePauseEvent.OnEventRaised += ToggleActionMap;
        // onPlayerDeath.OnEventRaised += onCreditsOrDeathEvent;

        // onCinematic = () => { SwitchActionMap(ActionMapName.Cinematics); };
        // onCinematicStartEvent.OnEventRaised += onCinematic;

        // onInteract = () => { SwitchActionMap(ActionMapName.Interact); };
        // onPlayerInteractingEvent.OnEventRaised += onInteract;

        onPlayerInputMapChange.OnEventRaised += SwitchActionMap;
    }

    private void SwitchActionMap(string mapName = null)
    {
        mapName = mapName ?? ActionMapName.Player;
        playerInput.SwitchCurrentActionMap(mapName.ToString());
    }

    // private void SwitchActionMap(string mapName = null)
    // {
    //     mapName = mapName ?? ActionMapName.Player;
    //     playerInput.SwitchCurrentActionMap(mapName);
    // }

    private void OnDisable()
    {
        // onTogglePauseEvent.OnEventRaised -= ToggleActionMap;
        // onPlayerDeath.OnEventRaised -= onCreditsOrDeathEvent;
        // onCinematicStartEvent.OnEventRaised -= onCinematic;
        // onPlayerInteractingEvent.OnEventRaised -= onInteract;

        onPlayerInputMapChange.OnEventRaised -= SwitchActionMap;
    }
}
