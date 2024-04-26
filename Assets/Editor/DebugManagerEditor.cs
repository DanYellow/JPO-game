using UnityEditor;
using UnityEngine.SceneManagement;

public class DebugManagerEditor
{
    [MenuItem("**Debug**/Restart Scene #R")]
    private static void RestartScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    [MenuItem("**Debug**/Quit game #L")]
    private static void QuitGame()
    {
        // CurrentSceneManager.QuitGame();
    }
}
