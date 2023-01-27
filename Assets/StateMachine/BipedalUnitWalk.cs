using UnityEngine;
// https://www.youtube.com/watch?v=AD4JIXQDw0s
public class BipedalUnitWalk : StateMachineBehaviour
{
    private Transform player;
    private Rigidbody2D rb;
    private Collider2D collider;
    private BipedalUnitBoss bipedalUnitBoss;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = animator.GetComponent<Rigidbody2D>();
        collider = animator.GetComponent<Collider2D>();
        bipedalUnitBoss = animator.GetComponent<BipedalUnitBoss>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
            return;

        Vector2 target = new Vector2(player.position.x, rb.position.y);

        float moveSpeed = bipedalUnitBoss.enemyData.moveSpeed;
        moveSpeed *= (bipedalUnitBoss.isEnraged ? 1 : bipedalUnitBoss.enrageData.bonusFactor);

        rb.MovePosition(
            Vector2.MoveTowards(rb.position, target, moveSpeed * Time.fixedDeltaTime)
        );

        float attackRange = bipedalUnitBoss.enemyData.attackRange;
        if(bipedalUnitBoss.isEnraged) {
            attackRange += bipedalUnitBoss.enemyData.attackRange / 2; 
        }

        if (
            Vector2.Distance(player.position, rb.position) <= attackRange &&
            (player.position.y < collider.bounds.max.y && player.position.y > collider.bounds.min.y) &&
            !bipedalUnitBoss.isInvulnerable
        )
        {
            animator.SetTrigger("Attack");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger("Attack");
    }
}
