using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject mainMenuUI;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;

    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private FloatValue totalDistanceTravelled;

    private void OnEnable()
    {
        onGameOver.OnEventRaised += DisplayScreen;
    }

    private void DisplayScreen()
    {
        totalDistanceTravelled.CurrentValue += Mathf.Round(distanceTravelled.CurrentValue);
        gameOverUI.SetActive(true);
        ExtensionsEventSystem.UpdateSelectedGameObject(gameOverUI.GetComponentInChildren<Button>().gameObject);
    }

    private void Awake()
    {
        gameOverUI.SetActive(false);
    }

    public void GoBackToIndex()
    {
        gameOverUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (gameOverUI != null && gameOverUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            gameOverUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && gameOverUI.activeInHierarchy)
        {
            gameOverUI.GetComponentInChildren<Button>().Select();
        }
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= DisplayScreen;
    }
}
