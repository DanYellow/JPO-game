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
    private VoidEventChannel onResetLastCheckPoint;

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
    void Start()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised += DisplayGameOverScreen;
        onResetLastCheckPoint.OnEventRaised += HideGameOverScreen;
    }

    private void DisplayGameOverScreen()
    {
        playerHUDUI.SetActive(false);
        gameoverMenuUI.SetActive(true);
        EventSystemExtensions.UpdateSelectedGameObject(gameoverMenuUI.GetComponentInChildren<Button>().gameObject);
        onPlayerInputMapChange.Raise(ActionMapName.UI);
        // StartCoroutine(DisplayGameOverScreenProxy());
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
        playerHUDUI.SetActive(false);
        gameoverMenuUI.SetActive(true);
        EventSystemExtensions.UpdateSelectedGameObject(gameoverMenuUI.GetComponentInChildren<Button>().gameObject);
        onPlayerInputMapChange.Raise(ActionMapName.UI);
        yield return new WaitForSeconds(0.75f);

        // foreach (var item in gameoverMenuUI.GetComponentsInChildren<Button>())
        // {
        //     item.interactable = false;
        // }


        // foreach (var item in gameoverMenuUI.GetComponentsInChildren<Button>())
        // {
        //     item.interactable = true;
        // }
        // StartCoroutine(TriggerInputAction());
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

    public void HideGameOverScreen()
    {
        onPlayerInputMapChange.Raise(ActionMapName.Player);

        EventSystem.current.SetSelectedGameObject(null);
        gameoverMenuUI.SetActive(false);
    }

    private void OnDisable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= DisplayGameOverScreen;
        onResetLastCheckPoint.OnEventRaised -= HideGameOverScreen;
    }
}
