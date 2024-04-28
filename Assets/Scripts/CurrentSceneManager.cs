using UnityEngine;
using UnityEngine.SceneManagement;

public class CurrentSceneManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue isCarTakingDamage;

    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    void Start()
    {
        Application.targetFrameRate = 60;

        isCarTakingDamage.CurrentValue = false;
        hasReachMinimumTravelDistance.CurrentValue = false;
    }

    public void RestartGame() {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
