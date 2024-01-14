using UnityEngine;

public class MechaChaseBehaviour : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    private LookAtTarget lookAtTarget;

    private Enemy enemy;
    private MechaGolemBoss mechaGolemBoss;

    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;

    private bool hasFightStarted = false;

    private float throwAllSpikesAttackThreshold = 0.52f;

    private void OnEnable()
    {
        onTogglePauseEvent.OnEventRaised += FightStart;
    }

    private void FightStart(bool arg0)
    {
        hasFightStarted = arg0;
    }


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemy = animator.GetComponent<Enemy>();
        rb = animator.GetComponent<Rigidbody2D>();
        lookAtTarget = animator.GetComponent<LookAtTarget>();
        mechaGolemBoss = animator.GetComponent<MechaGolemBoss>();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        mechaGolemBoss.PrepareSpikesProxy();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasFightStarted) return;
        mechaGolemBoss.StartShieldGenerationChecking();

        if (mechaGolemBoss.needsToActivateShield)
        {
            animator.SetBool(AnimationStrings.isGuarding, true);
        }

        lookAtTarget.Face(target);

        float speed = enemyData.walkSpeed;

        if (Vector2.Distance(target.position, rb.position) < 15)
        {
            speed = enemyData.runSpeed;
        }

        if (Vector2.Distance(target.position, rb.position) < 25 && !mechaGolemBoss.isPlayerDead)
        {

            if (Vector2.Distance(target.position, rb.position) > 5)
            {
                Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }

            if (Vector2.Distance(target.position, rb.position) < 10 && (float)enemy.GetHealth() / enemy.GetMaxHealth() <= throwAllSpikesAttackThreshold)
            {
                mechaGolemBoss.ThrowAllSpikesProxy();
                return;
            }

            mechaGolemBoss.ThrowSpikeProxy();
        }

        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));
    }

    private void OnDisable()
    {
        onTogglePauseEvent.OnEventRaised -= FightStart;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaGolemBoss.StopShieldGenerationChecking();
        mechaGolemBoss.StopThrowAllSpikes();
        mechaGolemBoss.StopThrowSpike();
    }
}
