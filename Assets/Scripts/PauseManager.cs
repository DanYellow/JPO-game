using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PauseManager : MonoBehaviour
{
    public BoolEventChannel onTogglePauseEvent;

    public GameObject pauseMenuUI;

    bool isGamePaused = false;

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

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenuUI.SetActive(false);
        isGamePaused = false;
        onTogglePauseEvent.Raise(isGamePaused);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && pauseMenuUI.activeInHierarchy)
        {
            pauseMenuUI.GetComponentInChildren<Button>().Select();
        }
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);

        EventSystemExtensions.UpdateSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);

        isGamePaused = true;
        Time.timeScale = 0;
        onTogglePauseEvent.Raise(isGamePaused);
    }
}
