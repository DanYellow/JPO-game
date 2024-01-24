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
    [SerializeField]
    private float guardCheckCountDownInitVal = 4.95f;
    private float guardCheckCountDown;
    private bool hasFightStarted = false;
    private float throwAllSpikesAttackLifeThreshold = 0.52f;

    private float throwSpikeCountdown = 0;
    private float throwSpikeCountdownMax = 2.8f;

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
        mechaGolemBoss.canMove = true;
        mechaGolemBoss.canGuardCheck = true;

        mechaGolemBoss.PrepareSpikesProxy();

        target = GameObject.FindGameObjectWithTag("Player").transform;

        guardCheckCountDown = guardCheckCountDownInitVal;
        throwSpikeCountdown = throwSpikeCountdownMax;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasFightStarted || mechaGolemBoss.isExpulsingSpikes) return;

        guardCheckCountDown -= Time.deltaTime;
        throwSpikeCountdown -= Time.deltaTime;
        if (guardCheckCountDown <= 0 && mechaGolemBoss.canGuardCheck)
        {
            guardCheckCountDown = guardCheckCountDownInitVal;
            animator.SetBool(AnimationStrings.isGuarding, Random.value <= 0.45f);
        }

        lookAtTarget.Face(target);

        float speed = enemyData.walkSpeed;

        if (Vector2.Distance(target.position, rb.position) < 15)
        {
            speed = enemyData.runSpeed;
        }

        if (Vector2.Distance(target.position, rb.position) < 25 && !mechaGolemBoss.isPlayerDead)
        {
            if (Vector2.Distance(target.position, rb.position) > 8 && mechaGolemBoss.canMove)
            {
                Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }

            if (throwSpikeCountdown <= 0)
            {
                throwSpikeCountdown = throwSpikeCountdownMax;
                if (
                    Vector2.Distance(target.position, rb.position) < 8 &&
                    (float)enemy.GetHealth() / enemy.GetMaxHealth() <= throwAllSpikesAttackLifeThreshold
                )
                {
                    mechaGolemBoss.ThrowAllSpikesRoutine();
                }
                else
                {
                    mechaGolemBoss.ThrowSpikeRoutine();
                }
            }


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
        // mechaGolemBoss.StopThrowAllSpikes();
        // mechaGolemBoss.StopThrowSpike();

        mechaGolemBoss.StopAllCoroutines();
    }
}
