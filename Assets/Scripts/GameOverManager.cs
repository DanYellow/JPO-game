using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;
    public GameObject gameoverMenuUI;
    public GameObject playerHUDUI;

    private void Awake()
    {
        gameoverMenuUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised += DisplayGameOverScreen;
    }

    private void DisplayGameOverScreen()
    {
        gameoverMenuUI.SetActive(true);
        playerHUDUI.SetActive(false);

        EventSystemExtensions.UpdateSelectedGameObject(gameoverMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && gameoverMenuUI.activeInHierarchy)
        {
            gameoverMenuUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void HideGameOverScreen()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameoverMenuUI.SetActive(false);
    }

    private void OnDestroy()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= DisplayGameOverScreen;
    }
}
