using UnityEngine;

public class MechaGuardBehaviour : StateMachineBehaviour
{
    private MechaProtect mechaProtect;
    private MechaGolemBoss mechaGolemBoss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect = animator.GetComponent<MechaProtect>();
        mechaProtect.shield.SetActive(true);
        mechaProtect.isGuarding = true;
        mechaProtect.enabled = true;


        mechaGolemBoss = animator.GetComponent<MechaGolemBoss>();
        mechaGolemBoss.PrepareSpikesProxy();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaGolemBoss.ExpulseSpikesProxy();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect.shield.SetActive(false);
        mechaGolemBoss.StopExpulseSpikes();
        mechaProtect.isGuarding = false;
        mechaProtect.enabled = false;
    }
}
