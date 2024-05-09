using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using TMPro;
using System.Text.RegularExpressions;


public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private GameObject gameOverUI;

    [SerializeField]
    private GameObject mainMenuUI;

    [SerializeField]
    private GameObject resultTextContainer;

    private TextMeshProUGUI resultText;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private FloatValue totalDistanceTravelled;

    [SerializeField]
    private CameraShakeType gameOverCameraShake;

    [SerializeField]
    private StringValue startTimeGame;

    private void Awake()
    {
        gameOverUI.SetActive(false);
        resultText = resultTextContainer.GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        onGameOver.OnEventRaised += DisplayScreen;
    }

    private void DisplayScreen()
    {
        onCinemachineShake.Raise(gameOverCameraShake);
        totalDistanceTravelled.CurrentValue += Mathf.Round(distanceTravelled.CurrentValue);
        UpdateResult();

        gameOverUI.SetActive(true);
        ExtensionsEventSystem.UpdateSelectedGameObject(gameOverUI.GetComponentInChildren<Button>().gameObject);
    }

    private void UpdateResult()
    {
        // User score
        string userDistance = Regex.Match(resultText.text, "<color=#AAAAFF>(.*?)</color>").Groups[1].ToString();
        string userDistanceTagColor = Regex.Match(userDistance, "<color=#([A-z0-9]*)>").Groups[0].ToString();
        string userDistanceComputed = $"{userDistanceTagColor}{Mathf.Round(distanceTravelled.CurrentValue)} m";

        string userDistanceResultUpdated = resultText.text.Replace(userDistance, userDistanceComputed);
        // Total score
        string totalDistance = Regex.Match(userDistanceResultUpdated, "<color=#BBAAFF>(.*?)</color>").Groups[1].ToString();
        string totalDistanceTagColor = Regex.Match(totalDistance, "<color=#([A-z0-9]*)>").Groups[0].ToString();
        string totalDistanceComputed = $"{totalDistanceTagColor}{Mathf.Round(totalDistanceTravelled.CurrentValue)} m";

        string totalDistanceResultUpdated = userDistanceResultUpdated.Replace(totalDistance, totalDistanceComputed);

        // string finalString = totalDistanceResultUpdated.Replace("ce matin", startTimeGame.CurrentValue);

        resultText.SetText(totalDistanceResultUpdated);
    }

    // public void GoBackToIndex()
    // {
    //     gameOverUI.SetActive(false);
    //     mainMenuUI.SetActive(true);
    // }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (gameOverUI != null && gameOverUI.activeInHierarchy && ctx.phase == InputActionPhase.Performed && EventSystem.current.currentSelectedGameObject == null)
        {
            gameOverUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && gameOverUI.activeInHierarchy)
        {
            gameOverUI.GetComponentInChildren<Button>().Select();
        }
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= DisplayScreen;
    }
}
