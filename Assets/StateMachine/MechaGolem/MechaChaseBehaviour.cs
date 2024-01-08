using UnityEngine;

public class MechaChaseBehaviour : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    private LookAtTarget lookAtTarget;

    private IsGrounded isGrounded;
    private Enemy enemy;
    private MechaGolemBoss mechaGolemBoss;

    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;

    private bool hasFightStarted = false;

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
        isGrounded = animator.GetComponent<IsGrounded>();
        lookAtTarget = animator.GetComponent<LookAtTarget>();
        mechaGolemBoss = animator.GetComponent<MechaGolemBoss>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!hasFightStarted) return;
        mechaGolemBoss.StartShieldGenerationChecking();
        mechaGolemBoss.PrepareSpikesProxy();

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

        if (Vector2.Distance(target.position, rb.position) < 25)
        {

            if (Vector2.Distance(target.position, rb.position) > 5)
            {
                Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
                Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, speed * Time.fixedDeltaTime);
                rb.MovePosition(newPos);
            }

            if (
                Vector2.Distance(target.position, rb.position) < 10 &&
                (enemy.GetHealth() / enemy.GetMaxHealth() < 0.4f)
            )
            {
                // mechaGolemBoss.ThrowAllSpikesProxy();
            }
            else
            {
                mechaGolemBoss.ThrowSpikesProxy();
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
        mechaGolemBoss.StopShieldGenerationChecking();
    }
}
