using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    private Animator animator;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public GameObject deathEffectPrefab;
    
    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    public bool isInvulnerable { get; set; } = false;

    private void Awake() {
        // playerStatsValue.currentHealth = playerStatsValue.maxHealth;
        animator = GetComponent<Animator>();
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
        if (isInvulnerable) return;

        playerStatsValue.nbCurrentLifes = Mathf.Clamp(
            playerStatsValue.nbCurrentLifes - 1, 
            0, 
            playerStatsValue.nbMaxLifes
        );

        if (playerStatsValue.nbCurrentLifes <= 0)
        {
            // StartCoroutine(SlowTime());
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            onPlayerDeathVoidEventChannel.Raise();
            Destroy(gameObject);
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
}
