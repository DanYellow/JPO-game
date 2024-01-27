
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable iDamageable = other.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(1);
        }
    }
}
