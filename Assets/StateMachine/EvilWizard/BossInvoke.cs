using UnityEngine;

public class BossInvoke : StateMachineBehaviour
{
    private EvilWizard evilWizard;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        evilWizard = animator.GetComponent<EvilWizard>();
        evilWizard.Invoke();
        animator.SetBool(AnimationStrings.invoke, false);
    }
}
