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

    public bool isEnraged = false;


    private void Start()
    {
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(currentHealth / enemyData.maxHealth < enemyData.enrageThreshold && !isEnraged) {
            isEnraged = true;
            animator.SetTrigger("IsEnraged");
        }

        // int factor = isFacingRight ? -1 : 1;
        // Vector2 pushBackVector = new Vector2(
        //     transform.position.normalized.x,
        //     0
        // ) * factor;
        // rb.AddForce(pushBackVector * enemyData.knockbackForce, ForceMode2D.Impulse);dd
    }

    public void Shoot()
    {
        //    GameObject nextBeam = Instantiate(beam, firePoint.position, Quaternion.identity);
        //    nextBeam.transform.right = firePoint.right.normalized;
    }

    public void EnragedCallback() {
        animator.ResetTrigger("IsEnraged");
    }
}
