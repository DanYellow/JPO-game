using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
// https://www.youtube.com/watch?v=vqZjZ6yv1lA

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject infosMenu;

    [SerializeField]
    private PlayerInput pi;

    // string deviceLayoutName, controlPath;

    private void Awake()
    {
        Debug.Log("Time.timeScale" + Time.timeScale);
        Time.timeScale = 1f;
        infosMenu.SetActive(false);

        if (pi.currentControlScheme.Equals("Gamepad"))
        {
            EventSystemExtensions.UpdateSelectedGameObject(mainMenu.GetComponentInChildren<Button>().gameObject);
        }
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (mainMenu != null && mainMenu.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            mainMenu.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && mainMenu.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                mainMenu.GetComponentInChildren<Button>().Select();
            }
        }

        if (input.currentControlScheme.Equals("Gamepad") && infosMenu.activeInHierarchy)
        {
            infosMenu.GetComponentInChildren<Button>().Select();
            // EventSystemExtensions.UpdateSelectedGameObject(infosMenu.GetComponentInChildren<ScrollRect>().gameObject);
        }

        if (input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void LoadLevel(int index)
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void DisplayInfosMenu(bool isStartGame = false)
    {
        infosMenu.SetActive(true);
        Button button = infosMenu.GetComponentInChildren<Button>();
        TextMeshProUGUI textMeshProUGUI = button.GetComponentInChildren<TextMeshProUGUI>();
        button.onClick.RemoveAllListeners();
        if (isStartGame)
        {
            textMeshProUGUI.SetText("COMMENCER");
            button.onClick.AddListener(() => LoadLevel(1));
        }
        else
        {
            button.onClick.AddListener(() => HideInfosMenu());
        }

        EventSystemExtensions.UpdateSelectedGameObject(infosMenu.GetComponentInChildren<Button>().gameObject);
    }

    public void HideInfosMenu()
    {
        EventSystemExtensions.UpdateSelectedGameObject(mainMenu.GetComponentsInChildren<Button>()[1].gameObject);
        infosMenu.SetActive(false);
    }

    public void QuitGame()
    {
        if (!infosMenu.activeInHierarchy)
        {
#if UNITY_EDITOR
            Debug.Log("Quit game");
#endif
            Application.Quit();
        }
    }
}


