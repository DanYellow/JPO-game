using UnityEngine;

public class SecretBossFight : StateMachineBehaviour
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

        float laserAttackRange = secretBoss.canThrowArms ? secretBossData.laserAttackRange : 0;
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
        
        if (
            (
                Mathf.Abs(player.position.x - selfTransform.position.x) >= laserAttackRange ||
                (secretBoss.IsTargetInLaserRange(player.position) && (Vector2) secretBoss.secretBossTorso.transform.localPosition == secretBoss.initTorsoPosition)
            ) &&
            secretBoss.isReadyToShootLaser
        )
        {
            secretBoss.MoveToShootTarget(player.position, player.transform.GetComponent<BoxCollider2D>().bounds);
        }

    }
}
