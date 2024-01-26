using UnityEngine;

public class EvilWizardAttackBehaviour : StateMachineBehaviour
{
    private EvilWizard evilWizard;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        evilWizard = animator.GetComponent<EvilWizard>();
        evilWizard.canOperate = true;
    }

     override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        evilWizard.canOperate = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationStrings.attack);
        evilWizard.canOperate = true;
        evilWizard.attackCountdown = evilWizard.attackCountdownMax;
        evilWizard.isAttacking = false;
    }
}
