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

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (pauseMenuUI != null && pauseMenuUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            pauseMenuUI.GetComponentInChildren<Button>().Select();
        }
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
        Time.timeScale = 0;
        isGamePaused = true;
        pauseMenuUI.SetActive(true);
        EventSystemExtensions.UpdateSelectedGameObject(pauseMenuUI.GetComponentInChildren<Button>().gameObject);
        onTogglePauseEvent.Raise(isGamePaused);
    }
}
