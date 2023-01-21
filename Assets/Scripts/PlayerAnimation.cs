using UnityEngine;
using System;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private VectorEventChannel vectorEventChannel;

    [SerializeField]
    private BoolEventChannel jumpBoolEventChannel;
    [SerializeField]
    private BoolEventChannel isGroundedBoolEventChannel;

    [SerializeField]
    private BoolEventChannel isShootingEventChannel;

    [SerializeField]
    private BoolEventChannel fallingBoolEventChannel;

     [SerializeField]
    private BoolEventChannel isInWaterBoolEventChannel;

    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;
    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    private UnityAction<bool> onJumpEvent;
    private UnityAction<bool> onLandEvent;
    private UnityAction<bool> onShootEvent;
    private UnityAction<bool> onFallEvent;
    private UnityAction<bool> inWaterEvent;
    private UnityAction onHurtEvent;
    // https://forum.unity.com/threads/unsubscribe-from-an-event-using-a-lambda-expression.1287587/
    private void Awake()
    {
        animator = GetComponent<Animator>();

        onJumpEvent = (bool isJumping) => { animator.SetBool("IsJumping", isJumping); };
        onLandEvent = (bool isGrounded) => { animator.SetBool("IsGrounded", isGrounded); };
        onShootEvent = (bool isGrounded) => { animator.SetTrigger("IsShooting"); };
        onFallEvent = (bool isGrounded) => { animator.SetTrigger("IsFalling"); };
        onHurtEvent = () => { animator.SetTrigger("IsHurt"); };
        inWaterEvent = (bool isInWater) => { 
            animator.speed = isInWater ? (playerStatsValue.waterSpeedFactor * 1.75f) : playerStatsValue.speedFactor;
        };

        vectorEventChannel.OnEventRaised += UpdateMovement;
        jumpBoolEventChannel.OnEventRaised += onJumpEvent;
        isShootingEventChannel.OnEventRaised += onShootEvent;
        isGroundedBoolEventChannel.OnEventRaised += onLandEvent;
        fallingBoolEventChannel.OnEventRaised += onFallEvent;
        isHurtVoidEventChannel.OnEventRaised += onHurtEvent;
        isInWaterBoolEventChannel.OnEventRaised += inWaterEvent;
    }

    private void UpdateMovement(Vector3 direction)
    {
        animator.SetFloat("MoveDirectionX", Mathf.Abs(direction.x));
        animator.SetBool("IsCrouching", direction.y <= -0.25f);
        animator.SetBool("IsLookingUp", direction.y >= 0.25f);
    }

    private void OnDestroy()
    {
        vectorEventChannel.OnEventRaised -= UpdateMovement;
        jumpBoolEventChannel.OnEventRaised -= onJumpEvent;
        isShootingEventChannel.OnEventRaised -= onShootEvent;
        fallingBoolEventChannel.OnEventRaised -= onFallEvent;
        isGroundedBoolEventChannel.OnEventRaised -= onLandEvent;
        isHurtVoidEventChannel.OnEventRaised -= onHurtEvent;
        isInWaterBoolEventChannel.OnEventRaised -= inWaterEvent;
    }
}
