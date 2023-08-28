using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyStats enemyStats;

    [SerializeField]
    private int currentLifePoints;

    private void Start() {
        currentLifePoints = enemyStats.maxLifePoints;
    }

    public void TakeDamage(int damage)
    {
        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            enemyStats.maxLifePoints
        );

        if (currentLifePoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}