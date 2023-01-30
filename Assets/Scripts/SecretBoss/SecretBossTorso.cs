using UnityEngine;

public class SecretBossTorso : Enemy
{
    [SerializeField]
    private Sprite level1;

    [SerializeField]
    private Sprite level2;

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (currentHealth / enemyData.maxHealth < (2f / 3f))
        {
            sr.sprite = level1;
        }
        if (currentHealth / enemyData.maxHealth < (1f / 3f))
        {
            sr.sprite = level2;
        }
    }
}
