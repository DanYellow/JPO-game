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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth.CurrentValue = maxHealth.CurrentValue;
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

        currentHealth.CurrentValue = Mathf.Clamp(currentHealth.CurrentValue - damage, 0, maxHealth.CurrentValue);
        if (currentHealth.CurrentValue == 0)
        {
            GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
            Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        } else {
            isHurtVoidEventChannel.Raise();
        }

    }
}
