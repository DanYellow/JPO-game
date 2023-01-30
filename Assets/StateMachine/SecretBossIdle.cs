using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBossIdle : StateMachineBehaviour
{
    private Transform player;
    private Transform selfTransform;

    [SerializeField]
    private EnemyStatsValue secretBossData;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        selfTransform = animator.GetComponent<Transform>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            return;

        Vector2 target = new Vector2(player.position.x, selfTransform.position.y);

        float attackRange = secretBossData.attackRange;
      //   if (bipedalUnitBoss.isEnraged)
      //   {
      //       attackRange += secretBossData.attackRange / 2;
      //   }

      if (Vector2.Distance(player.position, selfTransform.position) <= attackRange)
        {
            animator.SetBool("LaserBeamAttack", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("OnStateExit");
    }
}
