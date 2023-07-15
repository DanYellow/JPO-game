using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public GameObject deathEffectPrefab;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    public bool isInvulnerable { get; set; } = false;

    private void Awake()
    {
        // playerStatsValue.currentHealth = playerStatsValue.maxHealth;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage();
        }

        // if (Input.GetKeyDown(KeyCode.F9))
        // {
        //     TakeDamage(float.MaxValue);
        // }
#endif
    }

    // Update is called once per frame
    public void TakeDamage()
    {
        // if (true) return;
        if (isInvulnerable) return;

        playerStatsValue.nbCurrentLifes = Mathf.Clamp(
            playerStatsValue.nbCurrentLifes - 1,
            0,
            playerStatsValue.nbMaxLifes
        );

        isHurtVoidEventChannel.Raise();
        if (playerStatsValue.nbCurrentLifes <= 0)
        {
            // StartCoroutine(SlowTime());
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            onPlayerDeathVoidEventChannel.Raise();
            Destroy(gameObject);
        } else {
            // StartCoroutine(HandleInvicibilityDelay());
            // StartCoroutine(InvicibilityFlash());
        }
    }

    public IEnumerator InvicibilityFlash()
    {
        while (isInvulnerable)
        {
            sr.color = new Color(1f, 1f, 1f, 0f);
            yield return new WaitForSeconds(playerStatsValue.invulnerableData.flashDelay);
            sr.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(playerStatsValue.invulnerableData.flashDelay);
        }
    }

    public IEnumerator HandleInvicibilityDelay()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(playerStatsValue.invulnerableData.time);
        isInvulnerable = false;
    }
}
