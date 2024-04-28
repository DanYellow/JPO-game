using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;

public class ActionMapName
{
    public static string Drive = "Drive";
    public static string UI = "UI";
    public static string UIGameOverAndCredits = "UIGameOverAndCredits";
    public static string Cinematics = "Cinematics";
}

public class InputManager : MonoBehaviour
{
    [SerializeField]
    public PlayerInput playerInput;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolEventChannel onTogglePause;

    // [SerializeField]
    // private VoidEventChannel onPlayerDeath;
    
    private void Awake()
    {
        onTogglePause.OnEventRaised += ToggleActionMap;
    }

    public void ToggleActionMap(bool isPaused)
    {
        if (isPaused)
        {
            SwitchActionMap(ActionMapName.UI);
        }
        else
        {
            SwitchActionMap(ActionMapName.Drive);
        }
    }

    private void SwitchActionMap(string mapName = null)
    {
        mapName = mapName ?? ActionMapName.Drive;
        playerInput.SwitchCurrentActionMap(mapName);
    }

    private void OnDisable()
    {
        onTogglePause.OnEventRaised -= ToggleActionMap;
    }
}