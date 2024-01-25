using UnityEngine;

public class EvilWizardAttackBehaviour : StateMachineBehaviour
{
    private EvilWizard evilWizard;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        evilWizard = animator.GetComponent<EvilWizard>();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationStrings.attack);
        evilWizard.attackCountdown = evilWizard.attackCountdownMax;
        evilWizard.isAttacking = false;
    }
}
