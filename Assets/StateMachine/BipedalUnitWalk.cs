using UnityEngine;
// https://www.youtube.com/watch?v=AD4JIXQDw0s
public class BipedalUnitWalk : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    public EnemyStatsValue enemyData;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       player = GameObject.FindGameObjectWithTag("Player").transform;
       rb = animator.GetComponent<Rigidbody2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        rb.MovePosition(
            Vector2.MoveTowards(rb.position, target, enemyData.moveSpeed * Time.fixedDeltaTime)
        );

        if(Vector2.Distance(player.position, rb.position) <= enemyData.attackRange) {
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       animator.ResetTrigger("Attack");
    }
}
