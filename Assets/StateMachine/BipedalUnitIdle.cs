using UnityEngine;
using System.Collections;
using System;

public class BipedalUnitIdle : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private BipedalUnitBoss bipedalUnitBoss;
    public EnemyStatsValue enemyData;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        bipedalUnitBoss = animator.GetComponent<BipedalUnitBoss>();
        bipedalUnitBoss.isInvulnerable = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(player == null)
            return;
    
        Vector2 target = new Vector2(player.position.x, player.position.y);
        if (Vector2.Distance(player.position, rb.position) <= enemyData.activationRange)
        {
            bipedalUnitBoss.StartCombat();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("CombatStarted");
        bipedalUnitBoss.isInvulnerable = false;
    }
}
