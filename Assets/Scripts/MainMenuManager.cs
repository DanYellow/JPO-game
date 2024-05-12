using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenuUI;

    [SerializeField]
    private GameObject mainMenuLight;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameStart;

    [SerializeField]
    private VoidEventChannel onCameraSwitch;

    [SerializeField]
    private FloatValue totalDistanceTravelled;

    [SerializeField]
    private BoolValue isRestartingGame;

    void Awake()
    {
        Application.targetFrameRate = 30;
        Time.timeScale = 1;

        // Cursor.visible = false;
        if (!PlayerPrefs.HasKey("start_time"))
        {
            totalDistanceTravelled.CurrentValue = 0;
            PlayerPrefs.SetString("start_time", System.DateTime.Now.ToString("HH:mm"));
        }
    }

    private void Start()
    {
        mainMenuUI.SetActive(true);
        ExtensionsEventSystem.UpdateSelectedGameObject(mainMenuUI.GetComponentInChildren<Button>().gameObject);

        if (isRestartingGame.CurrentValue)
        {
            isRestartingGame.CurrentValue = false;
            StartGame();
        }
    }

    public void StartGame()
    {
        // Cursor.visible = true;
        mainMenuUI.SetActive(false);
        mainMenuLight.SetActive(false);
        onGameStart.Raise();
        onCameraSwitch.Raise();
        Application.targetFrameRate = 60;
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (mainMenuUI != null && mainMenuUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            mainMenuUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void Quit()
    {

#if UNITY_EDITOR
        Debug.Log("Quit game");
#endif
        Application.Quit();
    }

    void OnApplicationQuit()
    {
        PlayerPrefs.DeleteKey("start_time");
        totalDistanceTravelled.CurrentValue = 0;
        isRestartingGame.CurrentValue = false;
    }
}
