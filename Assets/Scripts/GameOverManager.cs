using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;
    public GameObject gameoverMenuUI;
    public GameObject playerHUDUI;

    public TMP_Text timerText;

    [SerializeField]
    private ScoreIndicator[] listScoreIndicator;

    [SerializeField, Range(1, 60)]
    private int scoreThreshold = 42;

    private void Awake()
    {
        gameoverMenuUI.SetActive(false);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("Time spent : " + (int)Time.timeSinceLevelLoad);
        }
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised += DisplayGameOverScreen;
    }

    private void DisplayGameOverScreen()
    {
        StartCoroutine(DisplayGameOverScreenProxy());
    }

    private string GetGameTime()
    {
        float t = Time.timeSinceLevelLoad; // time since scene loaded

        int seconds = (int)(t % 60); // return the remainder of the seconds divide by 60 as an int
        t /= 60; // divide current time y 60 to get minutes
        int minutes = (int)(t % 60); //return the remainder of the minutes divide by 60 as an int
        t /= 60; // divide by 60 to get hours

        return string.Format("{0}:{1}", minutes.ToString("00"), seconds.ToString("00"));
    }

    IEnumerator DisplayGameOverScreenProxy()
    {
        yield return new WaitForSeconds(0.75f);

        foreach (var item in gameoverMenuUI.GetComponentsInChildren<Button>())
        {
            item.interactable = false;
        }

        timerText.SetText($"Vous avez tenu : <b>{GetGameTime()} secondes !</b>");
        gameoverMenuUI.SetActive(true);

        int nbScoreIndicatorsEarned = Mathf.FloorToInt(Time.timeSinceLevelLoad / scoreThreshold);

        int nbScoreIndicatorsMax = Mathf.Clamp(nbScoreIndicatorsEarned, 0, listScoreIndicator.Length);

        for (int i = 0; i <nbScoreIndicatorsMax; i++)
        {
            if(i > listScoreIndicator.Length) {
                yield break;
            }
            yield return new WaitForSeconds(0.75f);
            listScoreIndicator[i].Activate();
        }

        foreach (var item in gameoverMenuUI.GetComponentsInChildren<Button>())
        {
            item.interactable = true;
        }

        StartCoroutine(TriggerInputAction());
    }

    IEnumerator TriggerInputAction()
    {
        yield return new WaitForSeconds(0.5f);

        EventSystemExtensions.UpdateSelectedGameObject(gameoverMenuUI.GetComponentInChildren<Button>().gameObject);
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && gameoverMenuUI.activeInHierarchy)
        {
            gameoverMenuUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnArrowUsed() {
        if (gameoverMenuUI.activeInHierarchy)
        {
            // https://discussions.unity.com/t/if-button-highlighted/136532
            // gameoverMenuUI.GetComponentInChildren<Button>()
            // gameoverMenuUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnActionTriggered(InputAction.CallbackContext context)
    {

    }

    public void HideGameOverScreen()
    {
        EventSystem.current.SetSelectedGameObject(null);
        gameoverMenuUI.SetActive(false);
    }

    private void OnDisable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= DisplayGameOverScreen;
    }
}
