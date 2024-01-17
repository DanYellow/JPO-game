using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetLayerMask;

    [SerializeField]
    private EnemyData enemyData;

    private BoxCollider2D bc2d;

    [SerializeField]
    private bool isAttacking = false;
    private Animator animator;
    private EnemyPatrol enemyPatrol;
    //  public ContactFilter2D ContactFilter;
    public UnityEvent OnBegin, OnDone;

    [SerializeField]
    private float distance = 2f;

    public float attackCooldown = 0;
    float attackDelay = 5f;

    public bool canMove = true;

    private void Update() {
        if(attackCooldown > 0) {
            attackCooldown -= Time.deltaTime;
        }
    }

    public bool CanAttack() {
        return attackCooldown <= 0;
    }

    public void ResetTimer() {
        attackCooldown = attackDelay;
        StartCoroutine(ResetMovements());
    }

    IEnumerator ResetMovements() {
        yield return Helpers.GetWait(distance * 2f);
        canMove = true;
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        OnBegin?.Invoke();
        animator.SetTrigger(AnimationStrings.attack);
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        OnDone?.Invoke();
        yield return Helpers.GetWait(enemyData.attackRate);
        isAttacking = false;
    }
}
