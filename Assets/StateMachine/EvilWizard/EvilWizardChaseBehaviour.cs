using UnityEngine;

public class EvilWizardChaseBehaviour : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    private LookAtTarget lookAtTarget;
    private EvilWizard evilWizard;
    private IsGrounded isGrounded;

    [SerializeField]
    private EnemyData enemyData;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        isGrounded = animator.GetComponent<IsGrounded>();
        evilWizard = animator.GetComponent<EvilWizard>();
        lookAtTarget = animator.GetComponent<LookAtTarget>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lookAtTarget.Face(target);
        if (evilWizard.invoking || !isGrounded.isGrounded) return;

        evilWizard.invokeCountdown -= Time.deltaTime;
        evilWizard.attackCountdown -= Time.deltaTime;

        float speed = enemyData.walkSpeed;
        if (Vector2.Distance(target.position, rb.position) < 15)
        {
            speed = enemyData.runSpeed;
        }

        if (
                Vector2.Distance(target.position, rb.position) > 8 &&
                Vector2.Distance(target.position, rb.position) < 25
            )
        {
            Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
            var dir = (targetPos - rb.position).normalized * speed;
            rb.velocity = dir;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }

        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));

        if (evilWizard.invokeCountdown <= 0 && !evilWizard.invoking)
        {
            bool randVal = Random.value <= 0.5f;
            if (randVal)
            {
                animator.SetBool(AnimationStrings.invoke, true);
            }
            else
            {
                evilWizard.invokeCountdown = evilWizard.invokeCountdownMax;
            }
        }

        if (
            evilWizard.attackCountdown <= 0 &&
            Vector2.Distance(target.position, rb.position) < 15 &&
            !evilWizard.isAttacking
        )
        {
            evilWizard.isAttacking = true;
            bool randVal = Random.value <= 0.25f;

            if (randVal)
            {
                animator.SetTrigger(AnimationStrings.attack2);
            }
            else
            {
                animator.SetTrigger(AnimationStrings.attack);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.ResetTrigger(AnimationStrings.attack);
    }
}
