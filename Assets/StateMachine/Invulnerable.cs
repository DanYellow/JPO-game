using UnityEngine;

namespace StateMachine
{
    public class Invulnerable : StateMachineBehaviour
    {
        [SerializeField]
        InvulnerableDataValue invulnerableDataValue;

        private float timerInvulnerability = 0;
        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            // animator.SetLayerWeight(1, 1);
            timerInvulnerability = invulnerableDataValue.invulnerabiltyTime;
        }

        override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
        {
            if (timerInvulnerability > 0)
            {
                timerInvulnerability -= Time.deltaTime;
            }
            else
            {
                animator.SetBool("IsNotInvulnerable", true);
            }
        }

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetLayerWeight(1, 0);
            animator.SetBool("IsNotInvulnerable", false);
        }
    }
}
