using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseResumeManager : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuUI;

    private bool isGamePaused = false;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;

    private void Awake()
    {
        pauseMenuUI.SetActive(false);
    }

    public void TogglePause(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
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

    private void Resume()
    {
        Application.targetFrameRate = 60;
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        onTogglePauseEvent.Raise(isGamePaused);
        EventSystem.current.SetSelectedGameObject(null);
    }

    private void Pause()
    {
        Application.targetFrameRate = 30;
        Time.timeScale = 0;
        isGamePaused = true;
        pauseMenuUI.SetActive(true);
        // EventSystemExtensions.UpdateSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);
        onTogglePauseEvent.Raise(isGamePaused);
    }
}
