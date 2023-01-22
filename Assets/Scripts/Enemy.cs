using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject deathEffect;
    private Rigidbody2D rb;

    public EnemyStatsValue enemyData;
    private float currentHealth = 0f;
    private float maxHealth;

    private SpriteRenderer spriteRenderer;

    private bool isHurt = false;

    public float invincibilityFlashDelay = 0.2f;
    public float invincibilityTimeAfterHit = 0.75f;

    // List of contact points when something collides with that GameObject
    private ContactPoint2D[] listContacts = new ContactPoint2D[1];

    // Start is called before the first frame update
    void Awake()
    {
        maxHealth = enemyData?.maxHealth ?? 1f;
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        if (currentHealth == 0)
        {
            GameObject impact = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(impact, impact.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
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
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        other.GetContacts(listContacts);
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            StartCoroutine(SwitchRbBodyType());
            iDamageable.TakeDamage(enemyData.damage);
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
