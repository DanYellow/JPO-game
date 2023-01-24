using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject deathEffect;
    protected Rigidbody2D rb;

    public EnemyStatsValue enemyData;

     [ReadOnlyInspector, SerializeField]
    protected float currentHealth = 0f;
    protected float maxHealth;

    [SerializeField]
    private VoidEventChannel onDeathCallback = null;
    protected Animator animator;

    protected SpriteRenderer sr;

    private bool isHurt = false;

    public float invincibilityFlashDelay = 0.2f;
    public float invincibilityTimeAfterHit = 0.75f;

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
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - (damage * enemyData.defense), 0, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 100f) / 100f;
        if (currentHealth == 0)
        {
            GameObject impact = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(impact, impact.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
            onDeathCallback?.Raise();
        }
        else
        {
            StopCoroutine(HandleInvincibilityDelay());
            StopCoroutine(InvincibilityFlash());

            StartCoroutine(HandleInvincibilityDelay());
            StartCoroutine(InvincibilityFlash());
        }
    }

    public IEnumerator InvincibilityFlash()
    {
        while (isHurt)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        other.GetContacts(listContacts);
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            // StartCoroutine(SwidtchRbBodyType());
            iDamageable.TakeDamage(enemyData.damage);
        }

        if (other.transform.TryGetComponent<IPushable>(out IPushable iPushable))
        {
            iPushable.HitDirection(listContacts[0]);
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
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isHurt = false;
    }
}
