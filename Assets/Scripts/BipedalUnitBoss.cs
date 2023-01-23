using System.Collections;
using System;
using UnityEngine;

public class BipedalUnitBoss : Enemy, IDamageable
{
    private bool isFacingRight;

    [SerializeField]
    private GameObject beam;

    [SerializeField]
    private Transform firePoint;
    public bool isActive { get; set; } = false;

    [HideInInspector]
    public bool isEnraged = false;


    [HideInInspector]
    public bool isInvulnerable = false;

    private void Start()
    {
    }

    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        base.TakeDamage(damage);

        if (currentHealth / enemyData.maxHealth < enemyData.enrageThreshold && !isEnraged)
        {
            isEnraged = true;
            animator.SetTrigger("IsEnraged");
        }
    }

    public void Shoot()
    {
        GameObject nextBeam = Instantiate(beam, firePoint.position, Quaternion.identity);
        nextBeam.transform.right = firePoint.right.normalized;
        SpriteBeam spriteBeam = nextBeam.GetComponent<SpriteBeam>();
        spriteBeam.moveSpeed = enemyData.beamSpeed;
        spriteBeam.damage = enemyData.damage;
        spriteBeam.invoker = gameObject;
    }

    public void EnragedCallback()
    {
        animator.ResetTrigger("IsEnraged");
    }

    public void StartCombat()
    {
        IEnumerator StartCombatProxy()
        {
            yield return new WaitForSeconds(0.2f);
            animator.SetTrigger("CombatStarted");
        }

        StartCoroutine(StartCombatProxy());
    }
}
