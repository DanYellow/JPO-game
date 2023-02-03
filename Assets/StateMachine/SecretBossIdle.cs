using UnityEngine;

public class SecretBossIdle : StateMachineBehaviour
{
    private Transform player;
    private Transform selfTransform;

    [SerializeField]
    private SecretBossData secretBossData;

    private SecretBoss secretBoss;

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

        Vector2 target = new Vector2(player.position.x, selfTransform.position.y);


        float laserAttackRange = secretBossData.laserAttackRange;
        float armsAttackRange = secretBossData.armsAttackRange;

        if (
            Vector2.Distance(player.position, selfTransform.position) <= armsAttackRange && 
            secretBoss.isReadyToThrowArms &&
            secretBoss.canThrowArms &&
            secretBoss.IsTargetInArmsRange(player.position)
            )
        {
            secretBoss.ThrowArms();
        }

        // if (Vector2.Distance(player.position, selfTransform.position) >= laserAttackRange && secretBoss.isReadyToShootLaser)
        // {
        //     secretBoss.MoveToShootTarget(player.position, player.transform.GetComponent<BoxCollider2D>().bounds);
        // }

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
