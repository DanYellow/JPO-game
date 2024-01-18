using UnityEngine;

public class GuardBehaviour : StateMachineBehaviour
{
    private Enemy enemy;

    private float guardCountdown = 0;
    [SerializeField]
    FloatValue guardDuration;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        guardCountdown = guardDuration.CurrentValue;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        guardCountdown -= Time.deltaTime;
        if (guardCountdown <= 0)
        {
            animator.SetBool(AnimationStrings.isGuarding, false);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy.ResetCanOperate(guardDuration.CurrentValue);
    }
}
