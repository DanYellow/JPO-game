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

        Debug.Log("input.currentControlScheme " + pi.currentControlScheme);

        // Debug.Log(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject);
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

    public void DisplayInfosMenu()
    {
        infosMenu.SetActive(true);
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


