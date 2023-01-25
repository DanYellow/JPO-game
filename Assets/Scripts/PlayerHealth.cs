using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public bool isSensitiveToLava { get; set; }
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    private Animator animator;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public GameObject deathEffectPrefab;
    
    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    private bool isInvulnerable = false;

    private void Awake() {
        playerStatsValue.currentHealth = playerStatsValue.maxHealth;
        animator = GetComponent<Animator>();
        isSensitiveToLava = playerStatsValue.isSensitiveToLava;
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
        #endif

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.F9))
        {
            TakeDamage(float.MaxValue);
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
            onPlayerDeathVoidEventChannel.Raise();
            Destroy(gameObject);
        } else {
            animator.SetLayerWeight(1, 1);
            isHurtVoidEventChannel.Raise();
        }
    }

    IEnumerator SlowTime()
    {
        Time.timeScale = 0.5f;
        yield return new WaitForSecondsRealtime(5.25f);
        Time.timeScale = 1f;
    }
}
