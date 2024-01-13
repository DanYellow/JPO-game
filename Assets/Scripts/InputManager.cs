using UnityEngine.InputSystem;
using UnityEngine;

public class ActionMapName
{
    public static string Player = "Player";
    public static string UI = "UI";
    public static string UIGameOverAndCredits = "UIGameOverAndCredits";
    public static string Cinematics = "Cinematics";
    public static string Interact = "Interact";
    public static string PlayerStunned = "PlayerStunned";
    public static string Loading = "Loading";
}

public class InputManager : MonoBehaviour
{
    private PlayerInput playerInput;

    [SerializeField]
    private StringEventChannel onPlayerInputMapChange;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        SwitchActionMap(ActionMapName.Player);
    }

    private void OnEnable() {
        onPlayerInputMapChange.OnEventRaised += SwitchActionMap;
    }

    private void SwitchActionMap(string mapName = null)
    {
        mapName = mapName ?? ActionMapName.Player;
        if(playerInput) 
            playerInput.SwitchCurrentActionMap(mapName.ToString());
    }

    private void OnDisable()
    {
        onPlayerInputMapChange.OnEventRaised -= SwitchActionMap;
    }
}
