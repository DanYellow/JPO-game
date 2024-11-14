using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class CurrentSceneManager : MonoBehaviour
{
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }
}