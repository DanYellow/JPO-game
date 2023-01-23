using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    private void Awake() {
        Time.timeScale = 1f;
        EventSystemExtensions.UpdateSelectedGameObject(mainMenu.GetComponentInChildren<Button>().gameObject);
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad"))
        {
            mainMenu.GetComponentInChildren<Button>().Select();
        }
    }

    public void LoadLevel(int index)
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


