using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class DebugManager : MonoBehaviour
{
    private void Update()
    {
// #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
// #endif
    }
}