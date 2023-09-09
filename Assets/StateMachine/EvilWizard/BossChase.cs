using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector2.Distance(target.position, rb.position) < 25 && animator.GetBool(AnimationStrings.canMove) == true)
        {
            Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
            var dir = (targetPos - rb.position).normalized * 3.5f;
            rb.velocity = dir;
        } else {
            rb.velocity = Vector2.zero;
        }
        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));
        // Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
        // Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, 2.5f * Time.fixedDeltaTime);

        // rb.MovePosition(newPos);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
