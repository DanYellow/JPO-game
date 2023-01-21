using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public GameObject deathEffectPrefab;

    public FloatValue currentHealth;
    public FloatValue maxHealth;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    private void Start() {
        playerStatsValue.currentHealth = playerStatsValue.maxHealth;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }

    // Update is called once per frame
    public void TakeDamage(float damage)
    {

        playerStatsValue.currentHealth = Mathf.Clamp(playerStatsValue.currentHealth - damage, 0, playerStatsValue.maxHealth);
        if (playerStatsValue.currentHealth == 0)
        {
            // StartCoroutine(SlowTime());
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            deathEffect.GetComponent<Animator>().speed = 0.5f;
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
}
