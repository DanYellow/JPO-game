using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://forum.unity.com/threads/moveposition-velocity.425450/

public class PatrollingBehaviour : StateMachineBehaviour
{
    EnemyPatrol enemyPatrol;
    Rigidbody2D rb;
    Vector2 lastPosition = Vector2.zero;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyPatrol = animator.GetComponent<EnemyPatrol>();
        rb = animator.GetComponent<Rigidbody2D>();

        lastPosition = rb.position;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb.MovePosition(rb.position + (Vector2)animator.transform.right * enemyPatrol.GetData().walkSpeed * Time.fixedDeltaTime);
        Vector2 trackVelocity = (rb.position - lastPosition) / Time.deltaTime;
        lastPosition = rb.position;
   
        // Debug.Log("trackVelocity " + trackVelocity);
        // rb.velocity = new Vector2(enemyPatrol.GetData().walkSpeed * animator.transform.right.x, rb.velocity.y); 
        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(trackVelocity.x));

        if (enemyPatrol.HasTouchedObstacle() && !enemyPatrol.isFlipping)
        {
            enemyPatrol.Flipp();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
