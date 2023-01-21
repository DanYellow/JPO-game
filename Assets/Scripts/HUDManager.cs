using TMPro;
using UnityEngine;
using System.Collections;

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

        foreach (TMP_Text text in listTexts)
            healthText.text = $"Energy {playerStatsValue.currentHealth.ToString()}";

    }

    // Start is called before the first frame update
    void Start()
    {
        isHurtVoidEventChannel.OnEventRaised += UpdateLifePoints;
    }

    private void UpdateLifePoints()
    {
        foreach (TMP_Text text in listTexts)
            healthText.text = $"Energy {playerStatsValue.currentHealth.ToString()}";

        // healthText.text = $"Energy {playerStatsValue.currentHealth.ToString()}";
    }

    private void OnDestroy()
    {
        isHurtVoidEventChannel.OnEventRaised -= UpdateLifePoints;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
