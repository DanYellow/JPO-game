using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    public GameObject deathEffectPrefab;

    public FloatValue currentHealth;
    public FloatValue maxHealth;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

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
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        } else {
            isHurtVoidEventChannel.Raise();
        }

    }
}
