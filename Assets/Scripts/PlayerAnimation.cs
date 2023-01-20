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

    private UnityAction<bool> onJumpEvent;
    private UnityAction<bool> onLandEvent;
    private UnityAction<bool> onShootEvent;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        onJumpEvent = (bool isJumping) => { animator.SetBool("IsJumping", isJumping); };
        onLandEvent = (bool isGrounded) => { animator.SetBool("IsGrounded", isGrounded); };
        onShootEvent = (bool isGrounded) => { animator.SetTrigger("IsShooting"); };

        vectorEventChannel.OnEventRaised += UpdateMovement;
        jumpBoolEventChannel.OnEventRaised += onJumpEvent;
        isShootingEventChannel.OnEventRaised += onShootEvent;
        isGroundedBoolEventChannel.OnEventRaised += onLandEvent;
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
        isGroundedBoolEventChannel.OnEventRaised -= onLandEvent;
    }
}
