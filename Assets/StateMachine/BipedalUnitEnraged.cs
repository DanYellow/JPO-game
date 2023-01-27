using UnityEngine;

public class BipedalUnitEnraged : StateMachineBehaviour
{
    BipedalUnitBoss bipedalUnitBoss;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bipedalUnitBoss = animator.GetComponent<BipedalUnitBoss>();
        bipedalUnitBoss.isInvulnerable = true;
        bipedalUnitBoss.enemyData.currentDefense *= bipedalUnitBoss.enrageData.bonusFactor; 
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bipedalUnitBoss.isInvulnerable = false;
    }
}
