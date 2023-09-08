using UnityEngine;

public class PlayerAttack : StateMachineBehaviour
{
    [SerializeField]
    private BoolValue playerCanMove;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCanMove.CurrentValue = false;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        playerCanMove.CurrentValue = true;
    }
}
