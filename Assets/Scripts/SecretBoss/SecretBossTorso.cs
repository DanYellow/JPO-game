using UnityEngine;
using System.Linq;

public class SecretBossTorso : Enemy
{
    [SerializeField]
    private Sprite level1;

    [SerializeField]
    private Sprite level2;

    [HideInInspector]
    public bool isInvulnerable = false;

    public override void TakeDamage(float damage)
    {
        // if (isInvulnerable) return;

        SecretBossData secretBossData = (SecretBossData)enemyData;

        base.TakeDamage(damage);
        foreach (var phase in secretBossData.listPhases)
        {
            if(currentHealth / enemyData.maxHealth < phase.threshold) {
                sr.sprite = phase.sprite;
            }
        }
    }
}
