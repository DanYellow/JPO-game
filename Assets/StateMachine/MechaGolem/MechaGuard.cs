using UnityEngine;

public class MechaGuard : StateMachineBehaviour
{
    [SerializeField]
    private LayerMask targetLayerMask;

    private RaycastHit2D hitObstacle;
    private BoxCollider2D bc2d;

    private float distance = 1f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bc2d = animator.GetComponent<BoxCollider2D>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // hitObstacle = Physics2D.Linecast(
        //     new Vector2(bc2d.bounds.min.x - 0.5f, bc2d.bounds.center.y),           
        //     new Vector2(bc2d.bounds.max.x + 0.5f, bc2d.bounds.center.y),
        //     targetLayerMask        
        // );

        Debug.DrawRay(new Vector2(bc2d.bounds.min.x - 0.5f, bc2d.bounds.center.y),           
            new Vector2(bc2d.bounds.max.x + 0.5f, bc2d.bounds.center.y));
    //    hitObstacle = Physics2D.BoxCast(
    //        new Vector2(bc2d.bounds.center.x - 0.5f, bc2d.bounds.center.y),
    //        bc2d.bounds.size,
    //        0,
    //        enemyPatrol.isFacingRight ? Vector2.right : Vector2.left,
    //        distance,
    //        targetLayerMask
    //    );
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}
}
