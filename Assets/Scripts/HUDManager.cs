using TMPro;
using UnityEngine;
using System.Collections;
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
        Debug.Log("playerStatsValue.currentHealth " + playerStatsValue.currentHealth);
        foreach (TMP_Text text in listTexts)
            text.text = $"Energy {playerStatsValue.currentHealth.ToString()}";
    }

    private void OnDisable()
    {
        isHurtVoidEventChannel.OnEventRaised -= UpdateLifePoints;
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateLifePoints();
    }
}
