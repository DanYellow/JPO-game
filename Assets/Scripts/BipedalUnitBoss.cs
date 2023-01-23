using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BipedalUnitBoss : Enemy, IDamageable
{
    private bool isFacingRight;

    [SerializeField]
    private GameObject beam;

    [SerializeField]
    private Transform firePoint;

    [HideInInspector]
    public bool isEnraged = false;

    [HideInInspector]
    public bool isInvulnerable = false;

    private void Start()
    {
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
}
