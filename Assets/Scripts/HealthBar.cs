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

    public int? maxLifePoints { private get; set; }

    void Start()
    {
        if (nameText != null)
        {
            nameText.SetText(enemyData.name);
        }
    }

    public void UpdateContent(int currentLifePoints)
    {
        int _maxLifePoints = maxLifePoints ?? enemyData.maxLifePoints;
        if (displayWhenFull || currentLifePoints != _maxLifePoints)
        {
            container.SetActive(true);
        }
        else
        {
            container.SetActive(false);
        }

        float rate = (float)currentLifePoints / _maxLifePoints;
        bar.fillAmount = rate;

        if (lifePointsText != null)
        {
            lifePointsText.SetText($"{currentLifePoints}/{_maxLifePoints}");
        }
    }

    public void Hide()
    {
        container.SetActive(false);
    }
}
