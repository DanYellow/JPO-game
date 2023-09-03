using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CurrentSceneManager : MonoBehaviour
{
    [SerializeField]
    private FloatValue timeBarValue;

    [SerializeField]
    private BoolValue playerIsDashing;

    private void Awake() {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        Initialize();
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

    public void RestartLastCheckpoint()
    {
        LoadLevel();
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
