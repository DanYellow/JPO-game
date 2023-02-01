using UnityEngine;

public class LightningCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (
            other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable) &&
            other.CompareTag("Player")
        )
        {
            iDamageable.TakeDamage(0);
        }
    }
}
