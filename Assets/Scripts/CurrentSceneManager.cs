using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

public class CurrentSceneManager : MonoBehaviour
{
    [SerializeField]
    private GameObject creditsUI;

    private UnityAction onDisplayCreditsScreen;

    [SerializeField]
    private FloatValue timeBarValue;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        // creditsUI.SetActive(false);
    }

    void Start()
    {
        onDisplayCreditsScreen = () => { StartCoroutine(DisplayCreditsScreen()); };
    }

    IEnumerator DisplayCreditsScreen()
    {
        yield return new WaitForSeconds(1);
        // creditsUI.SetActive(true);
        EventSystemExtensions.UpdateSelectedGameObject(creditsUI.GetComponentInChildren<Button>().gameObject);
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (creditsUI != null && creditsUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            creditsUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && creditsUI.activeInHierarchy)
        {
            creditsUI.GetComponentInChildren<Button>().Select();
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
