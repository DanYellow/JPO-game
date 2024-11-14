using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private bool isGrounded = false;

    private bool isFalling = false;

    private bool isGroundPounding = false;

    private float lastGroundPoundCooldown = 0;

    [SerializeField]
    private LayerMask listGroundLayers;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField, Range(5, 20)]
    private float dropForce = 10;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private void FixedUpdate()
    {
        isFalling = IsFalling();
        isGrounded = IsGrounded();

        if (!isGrounded && !isGroundPounding)
        {
            Vector3 vel = rb.velocity;
            vel.y -= 10.5f * Time.deltaTime;
            rb.velocity = vel;
        }
    }

    public bool IsFalling()
    {
        return rb.velocity.y < 0;
    }

    public bool IsGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, playerData.groundCheckRadius, listGroundLayers).Length > 0;
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (Time.time - lastGroundPoundCooldown < playerData.groundPoundCooldown)
            {
                return;
            }

            if (!isGrounded)
            {
                StopAndSpin();
            }

            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, playerData.jumpForce, rb.velocity.z);
            }
        }
    }

    private void StopAndSpin()
    {
        isGroundPounding = true;
        lastGroundPoundCooldown = Time.time;

        StartCoroutine(DropAndPound());
    }

    private IEnumerator DropAndPound()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
        yield return Helpers.GetWait(0.15f);
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Vector3.down * playerData.dropForce, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        }
    }
}
