using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject infosMenu;

    [SerializeField]
    private PlayerInput pi;

    private void Awake()
    {
        Time.timeScale = 1f;
        infosMenu.SetActive(false);

        if (pi.currentControlScheme.Equals("Gamepad"))
        {
            EventSystemExtensions.UpdateSelectedGameObject(mainMenu.GetComponentInChildren<Button>().gameObject);
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && mainMenu.activeInHierarchy)
        {
            mainMenu.GetComponentInChildren<Button>().Select();
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
        if(isStartGame) {
            Button button = infosMenu.GetComponentInChildren<Button>();
            TextMeshProUGUI textMeshProUGUI = button.GetComponentInChildren<TextMeshProUGUI>();
            textMeshProUGUI.SetText("COMMENCER");
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => LoadLevel(1));
        }

        EventSystemExtensions.UpdateSelectedGameObject(infosMenu.GetComponentInChildren<Button>().gameObject);
    }

    public void HideInfosMenu()
    {
        EventSystemExtensions.UpdateSelectedGameObject(mainMenu.GetComponentInChildren<Button>().gameObject);
        infosMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


