using System.Collections;
using UnityEngine;

public class BlastEffect : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private BlastEffectData blastEffectData;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    IEnumerator Start()
    {
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(blastEffectData.damage);
        }

        if (other.gameObject.TryGetComponent(out Knockback knockback))
        {
            knockback.Apply(gameObject, blastEffectData.knockbackForce);
        }
    }
}
