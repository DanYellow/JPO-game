using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossInvoke : StateMachineBehaviour
{

    private Rigidbody2D rb;
    private EvilWizard evilWizard;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        evilWizard = animator.GetComponent<EvilWizard>();
        evilWizard.Invoke();
        return;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // return;
        // Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
        // var dir = (targetPos - rb.position).normalized * 3.5f;
        // rb.velocity = dir;
        // animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));
        // Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
        // Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, 2.5f * Time.fixedDeltaTime);

        // rb.MovePosition(newPos);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log("Helloppz");
        animator.ResetTrigger(AnimationStrings.invoke);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
