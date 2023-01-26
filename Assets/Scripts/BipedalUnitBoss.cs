using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BipedalUnitBoss : Enemy, IDamageable
{
    private bool isFacingRight;

    [SerializeField]
    private GameObject beam;

    [SerializeField]
    private Transform firePoint;
    public bool isActive { get; set; } = false;
    public UnityEvent onStartCombat;

    [HideInInspector]
    public bool isEnraged = false;

    [HideInInspector]
    public bool isInvulnerable = false;

    public override void Awake()
    {
        base.Awake();
        gameObject.SetActive(false);
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        base.TakeDamage(isEnraged ? damage / enemyData.enrageFactor : damage);

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
        spriteBeam.moveSpeed = (isEnraged ? enemyData.enrageFactor : 1) * enemyData.beamSpeed;
        spriteBeam.damage = enemyData.damage * enemyData.enrageFactor;
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
            onStartCombat.Invoke();
            yield return new WaitForSeconds(0.2f);
            animator.SetTrigger("CombatStarted");
        }

        StartCoroutine(StartCombatProxy());
    }
}
