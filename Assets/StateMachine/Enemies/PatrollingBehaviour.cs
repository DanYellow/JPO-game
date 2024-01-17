using UnityEngine;

// https://forum.unity.com/threads/moveposition-velocity.425450/

public class PatrollingBehaviour : StateMachineBehaviour
{
    EnemyPatrol enemyPatrol;
    Rigidbody2D rb;
    bool hasCollisionWithObstacle = false;
    bool hasDetectedEnemy = false;
    bool hasEnemyInAttackRange = false;
    bool canAttack = true;
    EnemyAttack enemyAttack;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyPatrol = animator.GetComponent<EnemyPatrol>();
        rb = animator.GetComponent<Rigidbody2D>();
        enemyAttack = animator.GetComponent<EnemyAttack>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasCollisionWithObstacle = enemyPatrol.HasTouchedObstacle();
        hasEnemyInAttackRange = enemyPatrol.HasEnemyInAttackRange();
        hasDetectedEnemy = enemyPatrol.HasDetectedEnemy();

        // rb.MovePosition(rb.position + (Vector2)animator.transform.right * enemyPatrol.GetData().walkSpeed * Time.fixedDeltaTime);
        // trackVelocity = (rb.position - enemyPatrol.lastPosition) / Time.deltaTime;
        // enemyPatrol.lastPosition = rb.position;

        float moveSpeed = hasDetectedEnemy ? enemyPatrol.GetData().walkSpeed : enemyPatrol.GetData().runSpeed;

        
        if (enemyAttack.canMove)
        {
            rb.velocity = new Vector2(
                (hasCollisionWithObstacle ? 0 : moveSpeed) * animator.transform.right.x,
                rb.velocity.y
            );
        } else {
            rb.velocity = Vector2.zero;
        }

        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));

        if (hasEnemyInAttackRange && enemyAttack.CanAttack())
        {
            animator.SetTrigger(AnimationStrings.attack);
        }

        if (!hasEnemyInAttackRange && hasCollisionWithObstacle && !enemyPatrol.isFlipping)
        {
            enemyPatrol.Flipp();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
