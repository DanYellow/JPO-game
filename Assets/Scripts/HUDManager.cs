using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannel onHealthUpdated;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    [SerializeField]
    private Image healthBar;

    [SerializeField]
    private Gradient healthBarGradient;

    public GameObject playerHUDUI;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    [SerializeField]
    private TextMeshProUGUI lifePointsText;

    private GameObject barContainer;

    private GameObject cooldownContainer;
    private Image cooldownBackground;
    private TextMeshProUGUI cooldownText;

    [SerializeField]
    private StringEventChannel countdownEvent; 

    private void Awake()
    {
        playerHUDUI.SetActive(true);
        barContainer = playerHUDUI.transform.Find("BarContainer").gameObject;
        cooldownContainer = playerHUDUI.transform.Find("Special/Cooldown").gameObject;
        cooldownContainer.SetActive(false);
        cooldownBackground = cooldownContainer.GetComponent<Image>();

        cooldownText = cooldownContainer.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start() {
        UpdateHealth();
        lifePointsText.SetText($"{playerStatsValue.currentLifePoints}/{playerStatsValue.maxLifePoints}");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        onHealthUpdated.OnEventRaised += UpdateHealth;
        countdownEvent.OnEventRaised += UpdateCountdown;
    }

    private void UpdateCountdown(string val)
    {
        if (int.TryParse(val, out int i)) {
            cooldownContainer.SetActive(i != 0);
            cooldownBackground.enabled = i != 0;
        }
        cooldownText.SetText(val);
    }

    public void StartGame()
    {
        UpdateHealth();
        barContainer.SetActive(true);
    }

    private void UpdateHealth(bool _tmp = false)
    {
        float rate = (float) playerStatsValue.currentLifePoints / playerStatsValue.maxLifePoints;
        healthBar.fillAmount = rate;
        healthBar.color = healthBarGradient.Evaluate(rate);
        lifePointsText.SetText($"{playerStatsValue.currentLifePoints}/{playerStatsValue.maxLifePoints}");
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
    }

    private void OnDisable()
    {
        onHealthUpdated.OnEventRaised -= UpdateHealth;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        countdownEvent.OnEventRaised -= UpdateCountdown;
    }
}
