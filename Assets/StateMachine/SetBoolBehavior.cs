using UnityEngine;

public class SetBoolBehavior : StateMachineBehaviour
{
    [ReadOnlyInspector]
    private string animParameter = "CanMove";

    [SerializeField]
    private bool valueOnEnter;

    [SerializeField]
    private bool valueOnExit;

    [SerializeField]
    private bool updateState;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateState)
        {
            animator.SetBool(AnimationStrings.canMove, valueOnEnter);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (updateState)
        {
            animator.SetBool(AnimationStrings.canMove, valueOnExit);
        }
    }
}
