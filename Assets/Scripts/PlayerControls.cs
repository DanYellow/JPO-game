using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField, Range(1, 15)]
    private float jumpForce = 10;

    private bool isGrounded = false;

    private bool isFalling = false;

    private bool isGroundPounding = false;

    [SerializeField, Range(0, 2)]
    private float groundPoundCooldown = 2.75f;
    private float lastGroundPoundCooldown = 0;

    [SerializeField]
    private LayerMask listGroundLayers;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float groundCheckRadius;

    [SerializeField, Range(5, 20)]
    private float dropForce = 10;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

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
        return Physics.OverlapSphere(groundCheck.position, groundCheckRadius, listGroundLayers).Length > 0;
    }


    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (Time.time - lastGroundPoundCooldown < groundPoundCooldown)
            {
                return;
            }

            if (!isGrounded)
            {
                StopAndSpin();
            }

            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
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
        rb.AddForce(Vector3.down * dropForce, ForceMode.Impulse);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
