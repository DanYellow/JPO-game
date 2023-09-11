using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private GameObject container;

    public bool displayWhenFull = false;

    [SerializeField]
    private Image bar;

    [SerializeField]
    private EnemyData enemyData;

    [Header("Texts")]
    [SerializeField]
    private TextMeshProUGUI lifePointsText;
    [SerializeField]
    private TextMeshProUGUI nameText;

    void Start()
    {
        if(nameText != null) {
            nameText.SetText(enemyData.name);
        }
    }

    public void UpdateBar(int currentLifePoints)
    {
        if(displayWhenFull || currentLifePoints != enemyData.maxLifePoints) {
            container.SetActive(true);
        }
        float rate = (float)currentLifePoints / enemyData.maxLifePoints;
        bar.fillAmount = rate;
        if(lifePointsText != null) {
            lifePointsText.SetText($"{currentLifePoints}/{enemyData.maxLifePoints}");
        }
    }

    public void Hide() {
        container.SetActive(false);
    }
}
