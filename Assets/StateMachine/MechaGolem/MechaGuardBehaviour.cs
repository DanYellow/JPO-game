using UnityEngine;

public class MechaGuardBehaviour : StateMachineBehaviour
{
    private MechaProtect mechaProtect;
    private MechaGolemBoss mechaGolemBoss;

     private Transform target;
     private PlayerMovements playerMovements;
     private BoxCollider2D targetBc;
     private float trapCountdown = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect = animator.GetComponent<MechaProtect>();
        mechaProtect.shield.SetActive(true);
        mechaProtect.isGuarding = true;
        mechaProtect.enabled = true;
        target = GameObject.FindWithTag("Player").transform;

        targetBc = target.GetComponent<BoxCollider2D>();
        playerMovements = target.GetComponent<PlayerMovements>();
        mechaGolemBoss = animator.GetComponent<MechaGolemBoss>();
        mechaGolemBoss.PrepareSpikesProxy();
        mechaGolemBoss.StartExpulseSpikesChecking();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        trapCountdown -= Time.deltaTime;

        if(trapCountdown <= 0 && playerMovements.isGrounded) {
            trapCountdown = 1.5f;
            mechaGolemBoss.mechaBossSpikeSpawn.transform.position = new Vector3(
                targetBc.bounds.center.x,
                targetBc.bounds.min.y - mechaGolemBoss.spikeSpawnBounds.size.y / 2,
                0
            );
            mechaGolemBoss.mechaBossSpikeSpawn.SetActive(true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect.shield.SetActive(false);
        mechaGolemBoss.StopExpulseSpikes();
        mechaGolemBoss.StopExpulseSpikesChecking();
        mechaGolemBoss.mechaBossSpikeSpawn.GetComponent<MechaBossSpikeSpawn>().DestroyChild();
        mechaGolemBoss.mechaBossSpikeSpawn.SetActive(false);
        mechaProtect.isGuarding = false;
        mechaProtect.enabled = false;
    }
}
