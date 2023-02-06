using UnityEngine.SceneManagement;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField]
    private bool activateSlowTime = false;

    [SerializeField, Range(0, 1)]
    private float slowTime = 1f;
    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.ClearDeveloperConsole();
            RestartLevel();
        }
        if (activateSlowTime)
        {
            Time.timeScale = slowTime;
        }
        else
        {
            Time.timeScale = 1;
        }
#endif
    }

    private void RestartLevel(int levelName = 1)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DebugMessage(string message = "hello") {
        Debug.Log("Debug Manager: " + message);
    }

}
