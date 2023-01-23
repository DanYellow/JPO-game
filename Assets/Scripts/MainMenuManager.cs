using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    private void Awake() {
        Time.timeScale = 1f;
        mainMenu.GetComponentInChildren<Button>().Select();
    }

    public void LoadLevel(int index)
    {
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}


