using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Guard : MonoBehaviour, IGuardable
{
    private Animator animator;
    public UnityEvent OnBegin, OnDone;
    private BoxCollider2D bc2d;
    private EnemyPatrol enemyPatrol;

    [SerializeField]
    private float distance = 1f;

    [SerializeField]
    private LayerMask targetLayerMask;

    public bool isGuarding { get; set; } = false;

    private RaycastHit2D hitObstacle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    private void FixedUpdate()
    {
        hitObstacle = Physics2D.BoxCast(
           new Vector2(bc2d.bounds.center.x - 0.5f, bc2d.bounds.center.y),
           bc2d.bounds.size,
           0,
           enemyPatrol.isFacingRight ? Vector2.right : Vector2.left,
           distance,
           targetLayerMask
       );

        Debug.DrawRay(new Vector2(enemyPatrol.isFacingRight ? bc2d.bounds.max.x : bc2d.bounds.min.x, bc2d.bounds.min.y), (enemyPatrol.isFacingRight ? Vector2.right : Vector2.left) * distance, Color.cyan);
        Debug.DrawRay(new Vector2(enemyPatrol.isFacingRight ? bc2d.bounds.max.x : bc2d.bounds.min.x, bc2d.bounds.max.y), (enemyPatrol.isFacingRight ? Vector2.right : Vector2.left) * distance, Color.cyan);
        // Debug.DrawRay(new Vector2(bc2d.bounds.min.x - 0.25f, bc2d.bounds.max.y), enemyPatrol.isFacingRight ? Vector2.right : Vector2.left * distance, Color.cyan);

        if (hitObstacle)
        {
            if (!isGuarding)
            {
                StartCoroutine(Protect());
            }
            else
            {
                Reflect();
            }
        }
    }

    IEnumerator Protect()
    {
        isGuarding = true;
        OnBegin?.Invoke();
        animator.SetBool(AnimationStrings.isGuarding, isGuarding);
        yield return null;
        yield return Helpers.GetWait(2.15f);
        OnDone?.Invoke();
        isGuarding = false;
        animator.SetBool(AnimationStrings.isGuarding, isGuarding);
    }

    private void Reflect()
    {
        if ( hitObstacle.transform.right.x != transform.right.x && hitObstacle.transform.TryGetComponent(out Knockback knockback))
        {
            knockback.Apply(gameObject, 250);
        }
        // IAttackable iAttackable = hitObstacle.transform.GetComponentInChildren<IAttackable>();
        // if (iAttackable != null && iAttackable.isAttacking)
        // {
        //     if (hitObstacle.transform.TryGetComponent(out Knockback knockback))
        //     {
        //         knockback.Apply(gameObject, 250);
        //     }

        // }
    }

}
