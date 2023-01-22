using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;
    public GameObject gameoverMenuUI;
    public GameObject playerHUDUI;

    private void Awake()
    {
        gameoverMenuUI.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised += DisplayGameOverScreen;
    }

    private void DisplayGameOverScreen()
    {
        gameoverMenuUI.SetActive(true);
        playerHUDUI.SetActive(false);
        gameoverMenuUI.GetComponentInChildren<Button>().Select();
    }

    public void HideGameOverScreen()
    {
        gameoverMenuUI.SetActive(false);
    }

    private void OnDestroy()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= DisplayGameOverScreen;
    }
}
