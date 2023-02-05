
using UnityEngine;

public class SecretBossDisabled : StateMachineBehaviour
{
    private Transform player;
    private Transform selfTransform;

    [SerializeField]
    private SecretBossData secretBossData;

    private SecretBoss secretBoss;

    [SerializeField]
    private ShakeTypeValue activationShake;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        selfTransform = animator.GetComponent<Transform>();
        secretBoss = animator.GetComponent<SecretBoss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            return;

        if (Mathf.Abs(player.position.x - selfTransform.position.x) <= secretBossData.activationRange && !secretBoss.isActivating)
        {
            // animator.SetTrigger("CombatStarted");
            secretBoss.StartCombat();
        }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // animator.ResetTrigger("CombatStarted");
    }
}
