using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CurrentSceneManager : MonoBehaviour
{
    [SerializeField]
    private FloatValue timeBarValue;

    [SerializeField]
    private BoolValue playerIsDashing;

    [SerializeField]
    private Vector2Value lastCheckpoint;

    private void Awake()
    {
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

    public void LoadLevel(int levelIndex = 1)
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        if (levelIndex == 0)
        {
            lastCheckpoint.CurrentValue = null;
        }
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Single);
    }

    public void RestartLastCheckpoint()
    {
        LoadLevel();
    }

    public void QuitGame()
    {
        lastCheckpoint.CurrentValue = null;
        Application.Quit();
    }

    private void OnEnable()
    {
        timeBarValue.CurrentValue = 1f;
    }
}
