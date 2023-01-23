using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(float.MaxValue);
        }
    }
}
