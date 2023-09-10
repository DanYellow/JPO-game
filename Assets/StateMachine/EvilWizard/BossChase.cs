using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossChase : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    private LookAtTarget lookAtTarget;
    private EvilWizard evilWizard;
    private IsGrounded isGrounded;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        isGrounded = animator.GetComponent<IsGrounded>();
        evilWizard = animator.GetComponent<EvilWizard>();
        lookAtTarget = animator.GetComponent<LookAtTarget>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        animator.SetBool(AnimationStrings.invoke, false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lookAtTarget.Face(target);
        if (evilWizard.invoking || !isGrounded.isGrounded) return;
        if (
            Vector2.Distance(target.position, rb.position) > 10 &&
            Vector2.Distance(target.position, rb.position) < 25 &&
            animator.GetBool(AnimationStrings.canMove) == true
        )
        {
            Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
            var dir = (targetPos - rb.position).normalized * 3.5f;
            rb.velocity = dir;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));

        if (Vector2.Distance(target.position, rb.position) < 8 && evilWizard.canAttack)
        {
            animator.SetTrigger(AnimationStrings.attack);
        }

        if (rb.velocity == Vector2.zero && evilWizard.canInvoke && !evilWizard.invoking)
        {
            animator.SetBool(AnimationStrings.invoke, true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationStrings.attack);

    }
}
