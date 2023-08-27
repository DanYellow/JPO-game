using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using TMPro;

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

    private void Awake()
    {
        UpdateHealth();
        playerHUDUI.SetActive(true);
        barContainer = playerHUDUI.transform.Find("BarContainer").gameObject;
        // barContainer.SetActive(false);
    }

    private void Start() {
        lifePointsText.SetText($"{playerStatsValue.currentLifePoints}/{playerStatsValue.maxLifePoints}");
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        onHealthUpdated.OnEventRaised += UpdateHealth;

        onPause = (bool isPaused) =>
        {
            isGamePaused = isPaused;
        };
        onTogglePauseEvent.OnEventRaised += onPause;
    }

    public void StartGame()
    {
        UpdateHealth();
        barContainer.SetActive(true);
    }


    private void UpdateHealth()
    {
        float rate = (float) playerStatsValue.currentLifePoints / (float) playerStatsValue.maxLifePoints;
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
    }

    private void OnValidate()
    {
        // FillHearts();
    }

}
