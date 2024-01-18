using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour
{
    private new Collider2D collider;
    [SerializeField]
    private MyScriptableObject.Attack attackData;

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    private bool isRecovering = false;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        collider.enabled = false;

        StartCoroutine(ColliderCheck());
    }

    IEnumerator ColliderCheck()
    {
        while (true)
        {
            isRecovering = false;
            yield return new WaitUntil(() => collider.enabled == true);

            if (attackData.recoveryTime > 0 && GetComponentInParent<IStunnable>() != null && !isRecovering)
            {
                onCinemachineShake?.Raise(attackData.cameraShake);
                IStunnable iStunnable = GetComponentInParent<IStunnable>();
                yield return StartCoroutine(iStunnable.Stun(attackData.recoveryTime, EndAttack));
            }
        }
    }

    private void EndAttack()
    {
        isRecovering = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Knockback selfKnockback = GetComponentInParent<Knockback>();

        if (other.transform.TryGetComponent(out IGuardable iGuardable))
        {
            if (iGuardable.isGuarding && (other.transform.right.x != transform.right.x || iGuardable.hasTotalGuard))
            {
                selfKnockback.Apply(other.gameObject, KnockbackValues.lightAttack);
                return;
            }
        }

        if (other.transform.GetComponent<IReflectable>() != null)
        {
            selfKnockback.Apply(other.gameObject, 15);
        }

        IDamageable iDamageable = other.transform.GetComponent<IDamageable>();
        if (iDamageable != null)
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
