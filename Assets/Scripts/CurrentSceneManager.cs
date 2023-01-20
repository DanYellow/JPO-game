using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class CurrentSceneManager : MonoBehaviour
{
    public UnityAction OnEventRaised;

    void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            Time.timeScale = 1f;
            Debug.ClearDeveloperConsole();
            RestartLevel();
        }
        #endif
    }

    public void RestartLevel()
    {
        // OnEventRaised?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
