using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField]
    Animator animator;

    [SerializeField]
    VectorEventChannel vectorEventChannel;
    
    [SerializeField]
    BoolEventChannel jumpBoolEventChannel;
    [SerializeField]
    BoolEventChannel isGroundedBoolEventChannel;

    [SerializeField]
    BoolEventChannel isShootingEventChannel;


    private void Awake()
    {
        vectorEventChannel.OnEventRaised += UpdateMovement;
        jumpBoolEventChannel.OnEventRaised += (bool isJumping) => animator.SetBool("IsJumping", isJumping);
        isGroundedBoolEventChannel.OnEventRaised += (bool isGrounded) => animator.SetBool("IsGrounded", isGrounded);
        isShootingEventChannel.OnEventRaised += (bool isShooting) => animator.SetTrigger("IsShooting");
        // isShootingEventChannel.OnEventRaised += (bool isShooting) => animator.SetBool("IsGrounded", isShooting);
    }

    private void UpdateMovement(Vector3 direction)
    {
        animator.SetFloat("MoveDirectionX", Mathf.Abs(direction.x));
        animator.SetBool("IsCrouching", direction.y <= -0.25f);
        animator.SetBool("IsLookingUp", direction.y >= 0.25f);
    }
}
