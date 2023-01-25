using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    [SerializeField]
    private TMP_Text healthText;

    private TMP_Text[] listTexts;

    public GameObject playerHUDUI;

    private void Awake()
    {
        listTexts = playerHUDUI.GetComponentsInChildren<TMP_Text>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        isHurtVoidEventChannel.OnEventRaised += UpdateLifePoints;

        UpdateLifePoints();
    }

    private void UpdateLifePoints()
    {
        foreach (TMP_Text text in listTexts)
            text.text = $"Energy {Mathf.Ceil(playerStatsValue.currentHealth).ToString()}";
    }

    private void OnDisable()
    {
        isHurtVoidEventChannel.OnEventRaised -= UpdateLifePoints;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateLifePoints();
    }
}
