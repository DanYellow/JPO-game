using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private VectorEventChannel rbVelocityEventChannel;

    [SerializeField]
    private VoidEventChannel lightAttackEventChannel;

    [SerializeField]
    private BoolEventChannel playerCrouchEventChannel;

    [SerializeField]
    private BoolEventChannel onHealthUpdated;

    [SerializeField]
    private VoidEventChannel onPlayerDeath;

    [SerializeField]
    private BoolValue playerCanMove;
    private UnityAction<bool> playerCrouch;
    private UnityAction<bool> healthUpdated;

    private UnityAction onLightAttackEvent;
    private UnityAction onPlayerDeathEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        playerCrouch = (bool isCrouched) =>
        {
            animator.SetBool(AnimationStrings.isCrouched, isCrouched);
        };

        onLightAttackEvent = () =>
        {
            animator.SetTrigger(AnimationStrings.attack);
        };

        healthUpdated = (bool isTakingDamage) =>
        {
            if (isTakingDamage)
            {
                animator.SetTrigger(AnimationStrings.hurt);
            }
        };

        onPlayerDeathEvent = () =>
        {
            animator.SetBool(AnimationStrings.isDead, true);
        };
    }

    private void OnEnable()
    {
        rbVelocityEventChannel.OnEventRaised += UpdateMovement;
        lightAttackEventChannel.OnEventRaised += onLightAttackEvent;
        playerCrouchEventChannel.OnEventRaised += playerCrouch;
        onHealthUpdated.OnEventRaised += healthUpdated;

        onPlayerDeath.OnEventRaised += onPlayerDeathEvent;
    }

    private void UpdateMovement(Vector3 direction)
    {
        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(direction.x));
        // animator.SetFloat("VelocityY", direction.y);
        // animator.SetBool(AnimationStrings.isCrouched, true);
    }

    private void OnDisable()
    {
        rbVelocityEventChannel.OnEventRaised -= UpdateMovement;
        lightAttackEventChannel.OnEventRaised -= onLightAttackEvent;
        playerCrouchEventChannel.OnEventRaised -= playerCrouch;
        onHealthUpdated.OnEventRaised -= healthUpdated;
        onPlayerDeath.OnEventRaised -= onPlayerDeathEvent;
    }
}
