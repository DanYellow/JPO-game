using UnityEngine.InputSystem;
using UnityEngine;

public class ActionMapName
{
    public static string Drive = "Drive";
    public static string UI = "UI";
    public static string GameOverAndCredits = "GameOverAndCredits";
}

public class InputManager : MonoBehaviour
{
    [SerializeField]
    public PlayerInput playerInput;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolEventChannel onTogglePause;

    [SerializeField]
    private VoidEventChannel onStartGame;

    [SerializeField]
    private VoidEventChannel onGameOver;

    private void Awake()
    {
        onTogglePause.OnEventRaised += ToggleActionMap;
        onGameOver.OnEventRaised += OnGameOver;

        SwitchActionMap(ActionMapName.UI);
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
        Cursor.visible = isPaused;
    }

    private void SwitchActionMap(string mapName = null)
    {
        mapName = mapName ?? ActionMapName.Drive;
        playerInput.SwitchCurrentActionMap(mapName);
    }

    private void OnGameOver()
    {
        playerInput.SwitchCurrentActionMap(ActionMapName.GameOverAndCredits);
    }

    public void StartGame()
    {
        playerInput.SwitchCurrentActionMap(ActionMapName.Drive);
    }

    private void OnDisable()
    {
        onTogglePause.OnEventRaised -= ToggleActionMap;
        onGameOver.OnEventRaised -= OnGameOver;
    }
}