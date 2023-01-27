using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public bool isSensitiveToLava { get; set; }
    public GameObject deathEffect;
    protected Rigidbody2D rb;

    [SerializeField]
    public EnemyStatsValue enemyData;

    [ReadOnlyInspector, SerializeField]
    protected float currentHealth = 0f;
    protected float maxHealth;

    [SerializeField]
    private VoidEventChannel onDeathCallback = null;
    protected Animator animator;

    protected SpriteRenderer sr;

    private bool isHurt = false;

    // List of contact points when something collides with that GameObject
    private ContactPoint2D[] listContacts = new ContactPoint2D[1];

    // Start is called before the first frame update
    public virtual void Awake()
    {
        maxHealth = enemyData?.maxHealth ?? 1f;
        currentHealth = maxHealth;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isSensitiveToLava = enemyData.isSensitiveToLava;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - (damage * enemyData.currentDefense), 0, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 100f) / 100f;
        if (currentHealth == 0)
        {
            if (deathEffect)
            {
                GameObject impact = Instantiate(deathEffect, transform.position, Quaternion.identity);
                Destroy(impact, impact.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            }
            Destroy(gameObject);
            onDeathCallback?.Raise();
        }
        else
        {
            StopCoroutine(HandleInvincibilityDelay());
            StopCoroutine(InvincibilityFlash());
            animator.SetTrigger("IsHurt");
            animator.SetLayerWeight(1, 1);
            StartCoroutine(HandleInvincibilityDelay());
            StartCoroutine(InvincibilityFlash());
        }
    }

    public IEnumerator InvincibilityFlash()
    {
        while (isHurt)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(enemyData.invincibilityFlashDelay);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(enemyData.invincibilityFlashDelay);
        }
        animator.SetLayerWeight(1, 0);
        animator.ResetTrigger("IsHurt");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.GetContacts(listContacts);
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            // StartCoroutine(SwitchRbBodyType());
            iDamageable.TakeDamage(enemyData.damage);
        }

        if (other.transform.TryGetComponent<IPushable>(out IPushable iPushable))
        {
            iPushable.HitDirection(listContacts[0].normal);
        }
    }

    public IEnumerator SwitchRbBodyType()
    {
        rb.isKinematic = true;
        yield return new WaitForSeconds(0.5f);
        rb.isKinematic = false;
    }

    public IEnumerator HandleInvincibilityDelay()
    {
        isHurt = true;
        yield return new WaitForSeconds(enemyData.invincibilityTimeAfterHit);
        isHurt = false;
    }
}
