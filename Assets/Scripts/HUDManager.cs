using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using System;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onHealthUpdated;

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
    private BoolEventChannel onTogglePauseEvent;
    private UnityAction<bool> onPause;
    private bool isGamePaused = false;

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

        onPause = (bool isPaused) =>
        {
            isGamePaused = isPaused;
        };
        onTogglePauseEvent.OnEventRaised += onPause;
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


    private void UpdateHealth()
    {
        float rate = (float) playerStatsValue.currentLifePoints / playerStatsValue.maxLifePoints;
        healthBar.fillAmount = rate;
        healthBar.color = healthBarGradient.Evaluate(rate);
        lifePointsText.SetText($"{playerStatsValue.currentLifePoints}/{playerStatsValue.maxLifePoints}");
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // FillHearts();
    }

    private void OnDisable()
    {
        onHealthUpdated.OnEventRaised -= UpdateHealth;
        SceneManager.sceneLoaded -= OnSceneLoaded;
        onTogglePauseEvent.OnEventRaised -= onPause;
        countdownEvent.OnEventRaised -= UpdateCountdown;
    }

    private void OnValidate()
    {
        // FillHearts();
    }

}
