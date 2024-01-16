using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        // Vector2 dir = enemyPatrol.HasTouchedObstacle() ? Vector2.right 
    //    Vector2.MoveTowards(rb.position, Vector2.right, 2 * Time.fixedDeltaTime);
    float velocity = (rb.position - lastPosition).x / Time.fixedDeltaTime;
        // Debug.Log();
        rb.velocity = new Vector2(3 * animator.transform.right.x, rb.velocity.y);
    //    rb.MovePosition(rb.position + (Vector2)animator.transform.right * 2 * Time.fixedDeltaTime);
        // Debug.Log((rb.position - lastPosition).x /Time.fixedDeltaTime);
        // lastPosition = rb.position;
        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
