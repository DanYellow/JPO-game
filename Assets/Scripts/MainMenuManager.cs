using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject introUI;

    [SerializeField]
    private GameObject selectPlayerUI;


    void Awake()
    {
        Application.targetFrameRate = 30;
        Time.timeScale = 1;
        introUI.SetActive(true);
        selectPlayerUI.SetActive(false);
    }

    public void StartGame()
    {
        Application.targetFrameRate = 60;
        EventSystem.current.SetSelectedGameObject(null);
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void SelectPlayer() {
        introUI.SetActive(false);
        selectPlayerUI.SetActive(true);
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