using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour, IDamageable
{
    public Behaviour[] listIDamageableBehaviours;
    public bool isSensitiveToLava { get; set; }

    public void TakeDamage(float damage) {
        foreach (Behaviour component in listIDamageableBehaviours)
        {
            // Debug.Log("fzeezz " + component);
            component.gameObject.SendMessage("TakeDamage2", damage);
        }
    }
}
