using UnityEngine.InputSystem;
using UnityEngine;

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
        onPlayerInputMapChange.OnEventRaised += SwitchActionMap;
    }

    private void Start() {
        SwitchActionMap(ActionMapName.Player);
    }

    private void SwitchActionMap(string mapName = null)
    {
        mapName = mapName ?? ActionMapName.Player;
        playerInput.SwitchCurrentActionMap(mapName.ToString());
    }

    private void OnDisable()
    {
        onPlayerInputMapChange.OnEventRaised -= SwitchActionMap;
    }
}
