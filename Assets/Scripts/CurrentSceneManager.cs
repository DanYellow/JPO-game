using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class CurrentSceneManager : MonoBehaviour
{
    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.ClearDeveloperConsole();
            RestartLevel();
        }
        #endif
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
