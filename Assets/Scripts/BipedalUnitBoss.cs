using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BipedalUnitBoss : Enemy
{
    private bool isFacingRight;

    [SerializeField]
    private GameObject beam;

    [SerializeField]
    private GameObject beamEnraged;

    [SerializeField]
    public EnrageBehaviorValue enrageData;

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
        base.TakeDamage(isEnraged ? damage / enrageData.bonusFactor : damage);

        if (currentHealth / enemyData.maxHealth < enrageData.threshold && !isEnraged)
        {
            isEnraged = true;
            animator.SetTrigger("IsEnraged");
        }
    }

    public void Shoot()
    {
        GameObject nextBeam = Instantiate((isEnraged ? beamEnraged : beam), firePoint.position, Quaternion.identity);
        nextBeam.transform.right = firePoint.right.normalized;
        SpriteBeam spriteBeam = nextBeam.GetComponent<SpriteBeam>();
        spriteBeam.moveSpeedFactor = (isEnraged ? enrageData.bonusFactor : 1);
        spriteBeam.damageFactor = (isEnraged ? enrageData.bonusFactor : 1);
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
