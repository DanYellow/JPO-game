using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CurrentSceneManager : MonoBehaviour
{
    private UnityAction onDisplayCreditsScreen;

    [SerializeField]
    private FloatValue timeBarValue;

    // private SceneTransition sceneTransition;

    [SerializeField]
    private BoolValue playerIsDashing;

    private void Awake()
    {
        // sceneTransition = GetComponent<SceneTransition>();
    }

    void Start()
    {
        Application.targetFrameRate = 60;
        Initialize();
        // StartCoroutine(sceneTransition.Show());
    }

    private void Initialize()
    {
         timeBarValue.CurrentValue = 1f;
         playerIsDashing.CurrentValue = false;
    }

    public void LoadLevel(int levelName = 1)
    {
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
