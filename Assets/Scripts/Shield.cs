using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour, IDamageable
{
    [ReadOnlyInspector]
    private int currentLifePoints;

    private HealthBar healthBar;

    [SerializeField]
    private EnemyData enemyData;

    private void Awake()
    {
        healthBar = GetComponent<HealthBar>();
        healthBar.gameObject.SetActive(true);
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
            healthBar.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

}
