using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BipedalUnitBoss : Enemy, IDamageable
{

    SpriteRenderer[] normalizeSprites;    
    
    private bool isFacingRight;

    [SerializeField]
    private GameObject beam;

    [SerializeField]
    private Transform firePoint;

    private void Start()
    {
        EnrageMode();
        normalizeSprites = GetComponentsInChildren<SpriteRenderer>();
        Debug.Log("normalizeSprites " + normalizeSprites.Length);
        // GetComponentsInC$$anonymous$$ldren<SpriteRenderer>()
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        int factor = isFacingRight ? -1 : 1;
        Vector2 pushBackVector = new Vector2(
            transform.position.normalized.x,
            0
        ) * factor;
        rb.AddForce(pushBackVector * enemyData.knockbackForce, ForceMode2D.Impulse);
    }

    public void Shoot()
    {
        //    GameObject nextBeam = Instantiate(beam, firePoint.position, Quaternion.identity);
        //    nextBeam.transform.right = firePoint.right.normalized;
    }

    public void EnrageMode()
    {
        Debug.Log("EnrageMode");
        // sr.enabled = false;
        transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
        // sr.color = new Color(1, 0.25f, 0.5f, 1);
        // sr.color = new Color(1f, 1f, 1f, 0f);
    }
}
