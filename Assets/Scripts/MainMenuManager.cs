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
    private VoidEventChannel onStartGame;

    [SerializeField]
    private VoidEventChannel onCameraSwitch;
    [SerializeField]
    private StringValue startTimeGame;

    void Awake()
    {
        Application.targetFrameRate = 30;
        // Cursor.visible = false;
        startTimeGame.CurrentValue = System.DateTime.Now.ToString("HH:mm");
    }

    private void Start()
    {
        mainMenuUI.SetActive(true);
        ExtensionsEventSystem.UpdateSelectedGameObject(mainMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void StartGame()
    {
        // Cursor.visible = true;
        mainMenuUI.SetActive(false);
        mainMenuLight.SetActive(false);
        onStartGame.Raise();
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
}
