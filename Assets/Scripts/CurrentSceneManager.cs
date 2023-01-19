using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Events;

public class CurrentSceneManager : MonoBehaviour
{
    public UnityAction OnEventRaised;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.ClearDeveloperConsole();
            RestartLevel();
        }
    }

    public void RestartLevel()
    {
        OnEventRaised?.Invoke();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
