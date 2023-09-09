using UnityEngine;

public class PlayerAttack : StateMachineBehaviour
{
    // #nullable enable
    [SerializeField]
    private BoolValue playerCanMove;
    // #nullable disable

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.GetComponentInParent<Rigidbody2D>().velocity = Vector2.zero;
        if(playerCanMove != null) {

            playerCanMove.CurrentValue = false;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(playerCanMove != null) {
            playerCanMove.CurrentValue = true;
        }
    }
}
