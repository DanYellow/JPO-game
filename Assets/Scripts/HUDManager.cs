using UnityEngine;
using UnityEngine.UI;
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
    private Image damageBar;

    private float damageShrinkTimer;
    private float damageShrinkTimerMax = 0.95f;

    [SerializeField]
    private TextMeshProUGUI lifePointsText;

    private GameObject barContainer;

    private GameObject cooldownContainer;
    private Image cooldownBackground;
    private TextMeshProUGUI cooldownText;

    [SerializeField, Header("Events")]
    private StringEventChannel countdownEvent; 

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

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
        damageBar.fillAmount = healthBar.fillAmount;
        lifePointsText.SetText($"{playerStatsValue.currentLifePoints}/{playerStatsValue.maxLifePoints}");
    }

    private void Update()
    {
        damageShrinkTimer -= Time.deltaTime;
        if (damageShrinkTimer < 0 && healthBar.fillAmount < damageBar.fillAmount)
        {
            float shrinkSpeed = 1f;
            damageBar.fillAmount -= shrinkSpeed * Time.deltaTime;
        }
    }

    private void OnEnable()
    {
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
        damageShrinkTimer = damageShrinkTimerMax;
        float rate = (float) playerStatsValue.currentLifePoints / playerStatsValue.maxLifePoints;
        healthBar.fillAmount = rate;
        healthBar.color = healthBarGradient.Evaluate(rate);
        lifePointsText.SetText($"{playerStatsValue.currentLifePoints}/{playerStatsValue.maxLifePoints}");
    }

    private void OnDisable()
    {
        onHealthUpdated.OnEventRaised -= UpdateHealth;
        countdownEvent.OnEventRaised -= UpdateCountdown;
    }
}
