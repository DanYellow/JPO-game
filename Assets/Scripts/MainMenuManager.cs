using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject infosMenu;

    private void Awake() {
        Time.timeScale = 1f;
        infosMenu.SetActive(false);
        EventSystemExtensions.UpdateSelectedGameObject(mainMenu.GetComponentInChildren<Button>().gameObject);
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
    }

    public void LoadLevel(int index)
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void DisplayInfosMenu() {
        infosMenu.SetActive(true);
        EventSystemExtensions.UpdateSelectedGameObject(infosMenu.GetComponentInChildren<Button>().gameObject);
    }

    public void HideInfosMenu() {
        EventSystemExtensions.UpdateSelectedGameObject(mainMenu.GetComponentInChildren<Button>().gameObject);
        infosMenu.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


