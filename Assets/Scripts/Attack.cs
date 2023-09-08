using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    private new Collider2D collider;
    [SerializeField]
    private MyScriptableObject.Attack attackData;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.TryGetComponent(out IGuardable iGuardable))
        {
            if (iGuardable.isGuarding && other.transform.right.x != transform.right.x) return;
        }
        if (other.transform.TryGetComponent(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(attackData.damage);
        }
    }
}
