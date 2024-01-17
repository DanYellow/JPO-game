using UnityEngine;

// https://forum.unity.com/threads/moveposition-velocity.425450/

public class PatrollingBehaviour : StateMachineBehaviour
{
    EnemyPatrol enemyPatrol;
    Rigidbody2D rb;
    Vector2 trackVelocity = Vector2.zero;
    bool hasCollisionWithObstacle = false;
    bool hasCollisionWithEnemy = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyPatrol = animator.GetComponent<EnemyPatrol>();
        rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hasCollisionWithObstacle = enemyPatrol.HasTouchedObstacle();
        hasCollisionWithEnemy = enemyPatrol.HasTouchedEnemy();

        Debug.Log("hasCollisionWithEnemy " + hasCollisionWithEnemy);
        
        // rb.MovePosition(rb.position + (Vector2)animator.transform.right * enemyPatrol.GetData().walkSpeed * Time.fixedDeltaTime);
        // trackVelocity = (rb.position - enemyPatrol.lastPosition) / Time.deltaTime;
        // enemyPatrol.lastPosition = rb.position;
   
        // rb.velocity = new Vector2(
        //     (hasCollisionWithObstacle ? 0 : enemyPatrol.GetData().walkSpeed) * animator.transform.right.x, 
        //     rb.velocity.y
        // ); 

        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));

        if (hasCollisionWithObstacle && !enemyPatrol.isFlipping)
        {
            enemyPatrol.Flipp();
        } else {
            
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
