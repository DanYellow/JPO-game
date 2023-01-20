using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    public GameObject deathEffect;

    public FloatValue maxHealth;
    public float currentHealth = 0f;

    private SpriteRenderer spriteRenderer;

    private bool isHurt = false;

    public float invincibilityFlashDelay = 0.2f;
    public float invincibilityTimeAfterHit = 0.75f;

    // Start is called before the first frame update
    void Awake()
    {
        currentHealth = maxHealth?.CurrentValue ?? 1f;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth.CurrentValue);
        if (currentHealth == 0)
        {
            GameObject impact = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(impact, impact.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        } else {
            StopCoroutine(HandleInvincibilityDelay());
            StopCoroutine(InvincibilityFlash());

            StartCoroutine(HandleInvincibilityDelay());
            StartCoroutine(InvincibilityFlash());
        }
    }

    public IEnumerator InvincibilityFlash()
    {
        while(isHurt) {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    public IEnumerator HandleInvincibilityDelay()
    {
        isHurt = true;
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isHurt = false;
    }
}
