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


    private void Awake()
    {
        vectorEventChannel.OnEventRaised += UpdateMovement;
        // jumpBoolEventChannel.OnEventRaised += (bool isJumping) => {
        //     animator.SetTrigger("IsJumping2");
        //     Debug.Log("ffefzz");
        // };
        jumpBoolEventChannel.OnEventRaised += (bool isJumping) => animator.SetBool("IsJumping", isJumping);
        isGroundedBoolEventChannel.OnEventRaised += (bool isGrounded) => animator.SetBool("IsGrounded", isGrounded);
    }

    private void UpdateMovement(Vector3 direction)
    {
        animator.SetFloat("MoveDirectionX", Mathf.Abs(direction.x));
        // animator.SetBool("IsGrounded", isGrounded);
        // animator.SetBool("IsJumping", ctx.phase == InputActionPhase.Performed);
    }

    // private void UpdateJ
}
