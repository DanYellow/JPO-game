using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;
// https://www.youtube.com/watch?v=vqZjZ6yv1lA

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private PlayerInput pi;

    [SerializeField]
    private VoidEventChannel OnFirstLevelStart;

    private UnityAction onFirstLevelLoadEvent;

    private SceneTransition sceneTransition;

    private void Awake()
    {
        Time.timeScale = 1f;

        sceneTransition = GetComponent<SceneTransition>();
        // pi.enabled = false;
    }

    private void Start()
    {
        onFirstLevelLoadEvent = () =>
        {
            // LoadLevel(1); 
        };

        StartCoroutine(sceneTransition.Show());
        OnFirstLevelStart.OnEventRaised += onFirstLevelLoadEvent;
    }

    private void EnableControls() {
        pi.enabled = true;
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (mainMenu != null && mainMenu.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            mainMenu.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && mainMenu.activeInHierarchy)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                mainMenu.GetComponentInChildren<Button>().Select();
            }
        }

        if (input.currentControlScheme.Equals("Keyboard&Mouse"))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    public void LoadLevel(int index)
    {
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    public void TransitionToScene(int levelIndex) {
        StartCoroutine(sceneTransition.Hide());
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        Debug.Log("Quit game");
#endif
        Application.Quit();
    }

    private void OnDisable()
    {
        OnFirstLevelStart.OnEventRaised -= onFirstLevelLoadEvent;
    }
}


