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
    private BoolValue playerCanMove;
    private UnityAction<bool> playerCrouch;

    private UnityAction onLightAttackEvent;

    // https://forum.unity.com/threads/unsubscribe-from-an-event-using-a-lambda-expression.1287587/
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        rbVelocityEventChannel.OnEventRaised += UpdateMovement;

        onLightAttackEvent = () =>
        {
            if(playerCanMove.CurrentValue) {
                // animator.SetTrigger("LightAttack2");
                animator.SetBool(AnimationStrings.lightAttack, true);
            }
        };
        lightAttackEventChannel.OnEventRaised += onLightAttackEvent;


        playerCrouch = (bool isCrouched) => {
            animator.SetBool(AnimationStrings.isCrouched, isCrouched);
        };

        playerCrouchEventChannel.OnEventRaised += playerCrouch;
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
    }
}
