using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue isCarTakingDamage;

    [SerializeField]
    private BoolValue isRestartingGame;

    void Start()
    {
        isCarTakingDamage.CurrentValue = false;
    }

    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void ReloadGame() {
        isRestartingGame.CurrentValue = true;
        RestartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
