using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class CurrentSceneManager : MonoBehaviour
{
    private UnityAction onDisplayCreditsScreen;

    [SerializeField]
    private FloatValue timeBarValue;

    private SceneTransition sceneTransition;

    private void Awake()
    {
        sceneTransition = GetComponent<SceneTransition>();
    }

    void Start()
    {
        timeBarValue.CurrentValue = 1f;

        StartCoroutine(sceneTransition.Show());
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
