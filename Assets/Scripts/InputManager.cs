using UnityEngine.InputSystem;
using UnityEngine;

public class ActionMapName
{
    public static string Drive = "Drive";
    public static string Pause = "Pause";
    public static string MainMenuAndGameOver = "MainMenuAndGameOver";
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

    [SerializeField]
    private VoidEventChannel onGoBackToMainMenu;

    private void Awake()
    {
        onTogglePause.OnEventRaised += ToggleActionMap;
        onGameOver.OnEventRaised += OnGameOver;
        onGoBackToMainMenu.OnEventRaised += OnGameOver;

        SwitchActionMap(ActionMapName.Pause);
    }

    public void ToggleActionMap(bool isPaused)
    {
        if (isPaused)
        {
            SwitchActionMap(ActionMapName.Pause);
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
        playerInput.SwitchCurrentActionMap(ActionMapName.MainMenuAndGameOver);
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