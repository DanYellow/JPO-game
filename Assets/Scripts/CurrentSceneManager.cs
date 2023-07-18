using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class CurrentSceneManager : MonoBehaviour
{
    private UnityAction onDisplayCreditsScreen;

    [SerializeField]
    private FloatValue timeBarValue;

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    void Start()
    {
        timeBarValue.CurrentValue = 1f;
        
    }


    public void OnNavigate(InputAction.CallbackContext ctx)
    {

    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad"))
        {
            // creditsUI.GetComponentInChildren<Button>().Select();
        }
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
