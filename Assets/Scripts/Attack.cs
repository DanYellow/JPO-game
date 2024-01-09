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
        Knockback selfKnockback = GetComponentInParent<Knockback>();

        if (other.transform.TryGetComponent(out IGuardable iGuardable))
        {
            if (iGuardable.isGuarding && (other.transform.right.x != transform.right.x || iGuardable.hasTotalGuard))
            {
                return;
            }
        }

        if (other.transform.GetComponent<IReflectable>() != null)
        {
            selfKnockback.Apply(other.gameObject, 20);
        }

        if (other.transform.TryGetComponent(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(attackData.damage);
        }

        if (other.gameObject.TryGetComponent(out Knockback knockback))
        {
            knockback.Apply(gameObject, attackData.knockbackForce);
        }

        if (selfKnockback != null && other.transform.GetComponent<IReflectable>() == null)
        {
            selfKnockback.Apply(other.gameObject, -attackData.knockbackForce / 2);
        }
    }
}
