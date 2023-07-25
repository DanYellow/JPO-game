using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CurrentSceneManager : MonoBehaviour
{
    private UnityAction onDisplayCreditsScreen;

    [SerializeField]
    private FloatValue timeBarValue;

    private SceneTransition sceneTransition;

    [SerializeField]
    private PlayableDirector playableDirector;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        sceneTransition = GetComponent<SceneTransition>();
    }

    void Start()
    {
        timeBarValue.CurrentValue = 1f;

        StartCoroutine(sceneTransition.Show(() => { playableDirector.Play(); }));
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {

    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad"))
        {
            // creditsUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void LoadLevel(int levelName = 1)
    {
        Debug.Log("LoadLevel");
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        SceneManager.LoadScene(levelName, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnEnable()
    {
        timeBarValue.CurrentValue = 1f;
    }
}
