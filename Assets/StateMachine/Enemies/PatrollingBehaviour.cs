using UnityEngine;

// https://forum.unity.com/threads/moveposition-velocity.425450/

public class PatrollingBehaviour : StateMachineBehaviour
{
    EnemyPatrol enemyPatrol;
    Rigidbody2D rb;
    bool hasCollisionWithObstacle = false;
    bool hasDetectedEnemy = false;
    bool hasTouchedVoid = false;
    bool hasReachedLimitZone;
    RaycastHit2D enemyInAttackRange;
    Enemy enemy;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyPatrol = animator.GetComponent<EnemyPatrol>();
        rb = animator.GetComponent<Rigidbody2D>();
        enemy = animator.GetComponent<Enemy>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasCollisionWithObstacle = enemyPatrol.HasTouchedObstacle();
        enemyInAttackRange = enemyPatrol.HasEnemyInAttackRange();
        hasDetectedEnemy = enemyPatrol.HasDetectedEnemy();
        hasTouchedVoid = enemyPatrol.HasTouchedVoid();
        hasReachedLimitZone = enemyPatrol.HasReachedLimitZone();

        // rb.MovePosition(rb.position + (Vector2)animator.transform.right * enemyPatrol.GetData().walkSpeed * Time.fixedDeltaTime);
        // trackVelocity = (rb.position - enemyPatrol.lastPosition) / Time.deltaTime;
        // enemyPatrol.lastPosition = rb.position;
        float moveSpeed = hasDetectedEnemy ? enemyPatrol.GetData().runSpeed : enemyPatrol.GetData().walkSpeed;
  
        // No player detected
        if (enemyInAttackRange.collider == null || (enemyPatrol.enableLimitMovementDetection && !hasReachedLimitZone && enemy.canMove)) // 
        {
            if (!hasTouchedVoid)
            {
                rb.velocity = new Vector2(
                    (hasCollisionWithObstacle ? 0 : moveSpeed) * animator.transform.right.x,
                    rb.velocity.y
                );
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

            if ((hasCollisionWithObstacle || hasTouchedVoid) && !enemyPatrol.isFlipping)
            {
                enemyPatrol.Flip();
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));

        // Contact with target
        if (enemyInAttackRange.collider != null && !enemyPatrol.isFlipping)
        {
            bool isFacingEnemy = Mathf.Sign(enemyInAttackRange.collider.transform.right.x) != Mathf.Sign(animator.transform.right.x);

            if (enemy.canOperate == true)
            {
                enemy.canOperate = false;
                enemy.canMove = false;
                // animator.SetBool(AnimationStrings.isGuarding, true);
                bool randVal = UnityEngine.Random.value < 0.45f;
                if (randVal && isFacingEnemy && animator.ContainsParam(AnimationStrings.isGuarding))
                {
                    animator.SetBool(AnimationStrings.isGuarding, true);
                }
                else
                {
                    animator.SetTrigger(AnimationStrings.attack);
                }
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    // override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    // }
}
