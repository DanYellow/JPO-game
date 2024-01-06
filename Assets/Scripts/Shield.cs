using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IDamageable, IReflectable
{
    [ReadOnlyInspector]
    private int currentLifePoints;

    private HealthBar healthBar;

    [SerializeField]
    private GameObject healthBarContainer;

    [SerializeField]
    private EnemyData enemyData;

    public bool isReflecting { get; set; } = true;

    public bool hasTotalReflection => true;

    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();
        healthBarContainer.SetActive(true);
    }

    private void Start()
    {
        currentLifePoints = enemyData.maxLifePoints;

        healthBar.UpdateContent(currentLifePoints);
    }

    public void TakeDamage(int damage)
    {
        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            enemyData.maxLifePoints
        );

        healthBar.UpdateContent(currentLifePoints);

        if (currentLifePoints <= 0)
        {
            healthBarContainer.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
