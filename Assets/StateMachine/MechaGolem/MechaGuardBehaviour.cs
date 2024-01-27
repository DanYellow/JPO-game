using UnityEngine;

public class MechaGuardBehaviour : StateMachineBehaviour
{
    private MechaProtect mechaProtect;
    private MechaGolemBoss mechaGolemBoss;
    private Enemy enemy;
    private Transform target;
    private PlayerMovements playerMovements;
    private BoxCollider2D targetBc;
    private float trapCountdown = 0;
    private float trapCountdownMax = 3.5f;
    private float restoreHealthCountdown = 0;
    private float restoreHealthDelay = 7;

    private float expulseSpikeCountdown = 0;
    private float expulseSpikeCountdownMax = 3.05f;

    [SerializeField]
    private ParticleSystem healParticles;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mechaProtect = animator.GetComponent<MechaProtect>();
        mechaProtect.shield.SetActive(true);
        mechaProtect.isGuarding = true;
        mechaProtect.enabled = true;
        target = GameObject.FindWithTag("Player").transform;
        healParticles = animator.GetComponentInChildren<ParticleSystem>();

        enemy = animator.GetComponent<Enemy>();

        targetBc = target.GetComponent<BoxCollider2D>();
        playerMovements = target.GetComponent<PlayerMovements>();
        mechaGolemBoss = animator.GetComponent<MechaGolemBoss>();
        mechaGolemBoss.PrepareSpikesProxy();
        // mechaGolemBoss.StartExpulseSpikesChecking();
        // mechaGolemBoss.RotateSpikes(true);

        restoreHealthCountdown = restoreHealthDelay;
        expulseSpikeCountdown = expulseSpikeCountdownMax;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        trapCountdown -= Time.deltaTime;
        restoreHealthCountdown -= Time.deltaTime;
        expulseSpikeCountdown -= Time.deltaTime;

        float lifeRatio = enemy.GetHealthNormalized();
        bool isInRestoreHealthRange = lifeRatio >= 0.2f && lifeRatio <= 1f;
        
        if (restoreHealthCountdown <= 0 && isInRestoreHealthRange)
        {
            restoreHealthCountdown = restoreHealthDelay;
            if (!healParticles.isEmitting)
            {
                healParticles.Play();
            }
            enemy.RestoreHealth(3);
        }

        if (expulseSpikeCountdown <= 0)
        {
            // Debug.Log("Hllo");
            // expulseSpikeCountdown = expulseSpikeCountdownMax;
            // if (Random.value < 0.5f && !mechaGolemBoss.isExpulsingSpikes)
            // {
            //     mechaGolemBoss.ExpulseSpikesRoutine();
            // }
        }

        if (trapCountdown <= 0 && playerMovements.isGrounded)
        {
            trapCountdown = Mathf.Clamp(lifeRatio * trapCountdownMax, 1.5f, trapCountdownMax);
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
        mechaGolemBoss.ExpulseSpikesRoutine();
        mechaGolemBoss.mechaBossSpikeSpawn.GetComponent<MechaBossSpikeSpawn>().DestroyChild();
        mechaGolemBoss.mechaBossSpikeSpawn.SetActive(false);
        mechaProtect.isGuarding = false;
        mechaProtect.enabled = false;

        healParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
