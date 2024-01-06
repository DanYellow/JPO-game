using UnityEngine;

public class MechaGuard : StateMachineBehaviour
{
    [SerializeField]
    private LayerMask targetLayerMask;

    private RaycastHit2D hitObstacle;
    private BoxCollider2D bc2d;
    private LookAtTarget lookAtTarget;
    private Transform target;

    private MechaProtect mechaProtect;

    private float distance = 1f;

    public bool isGuarding { get; set; } = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        bc2d = animator.GetComponent<BoxCollider2D>();
        lookAtTarget = animator.GetComponent<LookAtTarget>();
        mechaProtect = animator.GetComponent<MechaProtect>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        isGuarding = true;

        mechaProtect.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    // override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    // {
    //     var origin = new Vector2(bc2d.bounds.min.x - 2.5f, bc2d.bounds.center.y);

    //     hitObstacle = Physics2D.Linecast(
    //         origin,           
    //         new Vector2(bc2d.bounds.max.x + 2.5f, bc2d.bounds.center.y),
    //         targetLayerMask        
    //     );

    //     if(hitObstacle) {
            
    //         Reflect(animator.gameObject);
    //     }
    //     lookAtTarget.Face(target);

        

    //     Debug.DrawRay(
    //         origin,
    //         (new Vector2(bc2d.bounds.max.x + 2.5f, bc2d.bounds.center.y) - origin),
    //         // Vector2.right * (bc2d.size.x + 2.5f), 
    //         Color.red
    //     );
    // }

    
    // private void Reflect(GameObject gameObject)
    // {
    //     if (hitObstacle.transform.TryGetComponent(out Knockback knockback))
    //     {
    //         knockback.Apply(gameObject, 5);
    //     }
    // }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect.enabled = false;
    //    isGuarding = false;
    }

    // private void OnDrawGizmos() {
    //     Gizmos.color = Color.blue;
    //     Gizmos.DrawLine(
    //         new Vector2(bc2d.bounds.min.x, bc2d.bounds.center.y), 
    //         new Vector2(bc2d.bounds.max.x, bc2d.bounds.center.y)
    //     );
    // }
}
