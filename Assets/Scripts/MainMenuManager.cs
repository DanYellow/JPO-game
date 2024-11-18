using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    void Awake()
    {
        Application.targetFrameRate = 30;
        Time.timeScale = 1;
    }

    public void StartGame()
    {
        Application.targetFrameRate = 60;
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }


    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        // if (mainMenuUI != null && mainMenuUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        // {
        //     mainMenuUI.GetComponentInChildren<Button>().Select();
        // }
    }

    public void Quit()
    {

#if UNITY_EDITOR
        Debug.Log("Quit game");
#endif
        Application.Quit();
    }

    private void OnDisable()
    {
    }

}