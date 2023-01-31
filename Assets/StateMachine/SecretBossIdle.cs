using UnityEngine;

public class SecretBossIdle : StateMachineBehaviour
{
    private Transform player;
    private Transform selfTransform;

    [SerializeField]
    private EnemyStatsValue secretBossData;

    private float nextShootTime = 0f;
    private float shootingRate = 6;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        selfTransform = animator.GetComponent<Transform>();
        nextShootTime = Time.time + shootingRate;
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

      if (Vector2.Distance(player.position, selfTransform.position) <= attackRange && Time.time >= nextShootTime)
        {
            animator.GetComponent<SecretBoss>().ShootBeam(player.position, player.transform.GetComponent<BoxCollider2D>().bounds);
            // animator.GetComponent<SecretBoss>().torso.transform.position = new Vector2(selfTransform.position.x, player.position.y);
            // animator.SetBool("LaserBeamAttack", true);
            nextShootTime = Time.time + shootingRate;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Debug.Log(  "OnStateExit");
    }
}
