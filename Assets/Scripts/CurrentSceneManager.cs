using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class CurrentSceneManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onBossKilled;

    [SerializeField]
    private GameObject creditsUI;

    private void Awake()
    {
        creditsUI.SetActive(false);
    }

    void Start()
    {
        onBossKilled.OnEventRaised += DisplayCreditsScreen;
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.ClearDeveloperConsole();
            RestartLevel();
        }
#endif
    }

    private void DisplayCreditsScreen()
    {
        creditsUI.SetActive(true);
        EventSystemExtensions.UpdateSelectedGameObject(creditsUI.GetComponentInChildren<Button>().gameObject);

    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && creditsUI.activeInHierarchy)
        {
            creditsUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void RestartLevel()
    {
        EventSystem.current.SetSelectedGameObject(null);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        onBossKilled.OnEventRaised -= DisplayCreditsScreen;
    }
}
