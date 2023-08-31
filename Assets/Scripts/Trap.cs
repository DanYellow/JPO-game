using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable iDamageable) && other.CompareTag("Player"))
        {
            iDamageable.TakeDamage(1);
        }
    }
}
