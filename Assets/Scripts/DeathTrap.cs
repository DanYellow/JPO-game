using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    private float nextDamageTime = 0;

    [SerializeField]
    private float damageTimeRate = 1f;

    [SerializeField]
    private float damagePerRateRate = 0.5f;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            if (iDamageable.isSensitiveToLava && Time.time >= nextDamageTime)
            {
                iDamageable.TakeDamage(damagePerRateRate);
                nextDamageTime = Time.time + damageTimeRate;
            }
        }
    }
}
