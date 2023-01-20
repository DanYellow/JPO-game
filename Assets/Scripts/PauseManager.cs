using UnityEngine;
using UnityEngine.UI;

public class PauseManager : MonoBehaviour
{
    public BoolEventChannel onTogglePauseEvent;

    public GameObject pauseMenuUI;

    bool isGamePaused = false;

    private void Awake() {
        pauseMenuUI.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGamePaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        onTogglePauseEvent.Raise(isGamePaused);
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        pauseMenuUI.GetComponentInChildren<Button>().Select();
        isGamePaused = true;
        Time.timeScale = 0;
        onTogglePauseEvent.Raise(isGamePaused);
    }
}
