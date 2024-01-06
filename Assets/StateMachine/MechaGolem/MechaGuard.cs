using UnityEngine;

public class MechaGuard : StateMachineBehaviour
{
    private MechaProtect mechaProtect;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect = animator.GetComponent<MechaProtect>();

        mechaProtect.enabled = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect.enabled = false;
    }
}
