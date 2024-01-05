using UnityEngine;
public class MechaChase : StateMachineBehaviour
{
    private Rigidbody2D rb;
    private Transform target;
    private LookAtTarget lookAtTarget;

    // private IsGrounded isGrounded;

    [SerializeField]
    EnemyData enemyData;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        rb = animator.GetComponent<Rigidbody2D>();
        // isGrounded = animator.GetComponent<IsGrounded>();
        lookAtTarget = animator.GetComponent<LookAtTarget>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        lookAtTarget.Face(target);

        float speed = enemyData.walkSpeed;
        if (Vector2.Distance(target.position, rb.position) < 15)
        {
            speed = enemyData.runSpeed;
        }

        Vector2 targetPos = new Vector2(target.position.x, rb.position.y);
        var dir = (targetPos - rb.position) * speed;
        rb.velocity = Vector2.left * 3;

        Debug.Log("rb.velocity " + rb.velocity);

        // if (
        //         Vector2.Distance(target.position, rb.position) > 10 &&
        //         Vector2.Distance(target.position, rb.position) < 25
        //     )
        // {

        //     Debug.Log(rb.velocity);
        // }
        // else
        // {
        //     rb.velocity = Vector2.zero;
        // }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
