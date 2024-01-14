using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using System.Collections;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    [SerializeField]
    private StringEventChannel onPlayerInputMapChange;

    public GameObject gameoverMenuUI;
    public GameObject playerHUDUI;

    private void Awake()
    {
        gameoverMenuUI.SetActive(false);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.N))
        {
            // print(EventSystem.current.currentSelectedGameObject);
            // gameoverMenuUI.GetComponentInChildren<Button>().Select();
            // Debug.Log("Time spent : " + (int)Time.timeSinceLevelLoad);
        }
#endif
    }

    // Start is called before the first frame update
    private void OnEnable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised += DisplayGameOverScreen;
    }

    private void DisplayGameOverScreen()
    {        
        onPlayerInputMapChange.Raise(ActionMapName.UIGameOverAndCredits);
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
        yield return new WaitForSeconds(0.85f);
        playerHUDUI.SetActive(false);
        gameoverMenuUI.SetActive(true);
        EventSystemExtensions.UpdateSelectedGameObject(gameoverMenuUI.GetComponentInChildren<Button>().gameObject);
        
    }

    public void OnControlsChanged(PlayerInput input)
    {
        if (input.currentControlScheme.Equals("Gamepad") && gameoverMenuUI.activeInHierarchy)
        {
            gameoverMenuUI.GetComponentInChildren<Button>().Select();
        }
    }

    public void OnNavigate(InputAction.CallbackContext ctx)
    {
        if (
            gameoverMenuUI != null &&
            gameoverMenuUI.activeInHierarchy &&
            ctx.phase == InputActionPhase.Performed &&
            EventSystem.current.currentSelectedGameObject == null
        )
        {
            gameoverMenuUI.GetComponentInChildren<Button>().Select();
        }
    }


    private void OnDisable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= DisplayGameOverScreen;
    }
}
