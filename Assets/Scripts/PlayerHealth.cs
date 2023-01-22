using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public GameObject deathEffectPrefab;
    
    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    private bool isInvulnerable = false;

    private void Awake() {
        playerStatsValue.currentHealth = playerStatsValue.maxHealth;
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
        #endif
    }

    // Update is called once per frame
    public void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        playerStatsValue.currentHealth = Mathf.Clamp(playerStatsValue.currentHealth - damage, 0, playerStatsValue.maxHealth);
        if (playerStatsValue.currentHealth == 0)
        {
            // StartCoroutine(SlowTime());
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
            onPlayerDeathVoidEventChannel.Raise();
        } else {
            isHurtVoidEventChannel.Raise();
        }
    }

    IEnumerator SlowTime()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(5.25f);
        Time.timeScale = 1f;
    }

    public IEnumerator HandleInvincibilityDelay()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(playerStatsValue.invulnerabiltyTime);
        isInvulnerable = false;
    }
}
