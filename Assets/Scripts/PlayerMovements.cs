using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    Animator animator;

    private Vector3 moveInput = Vector3.zero;

    private bool isFacingRight = true;
    public bool isGrounded;

    [Header("Events")]
    public VectorEventChannel vectorEventChannel;
    public BoolEventChannel jumpBoolEventChannel;
    public BoolEventChannel isGroundedBoolEventChannel;
    // public VectorEventChannel vectorEventChannel;

    [Space(15)]

    [Tooltip("Position checks")]
    public LayerMask listGroundLayers;
    public Transform groundCheck;
    public float groundCheckRadius;

    [Tooltip("Running system")]
    public float moveSpeed;

    [Header("Jump system")]
    public int jumpCount = 0;
    public int maxJumpCount;
    public float jumpForce;

    public PlayerInput playerInput;

    // Start is called before the first frame update
    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isGrounded && rb.velocity.y < 0.1f) {
            jumpBoolEventChannel.Raise(playerInput.actions["Jump"].WasReleasedThisFrame());
            jumpCount = 0;
        }

        vectorEventChannel.Raise(moveInput);
        isGroundedBoolEventChannel.Raise(isGrounded);
        
        Flip();
    }

    private void FixedUpdate() {
        rb.velocity = new Vector2((moveInput.x * moveSpeed), rb.velocity.y);

        isGrounded = IsGrounded();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if(ctx.phase == InputActionPhase.Performed && jumpCount < maxJumpCount) {
            jumpBoolEventChannel.Raise(ctx.phase == InputActionPhase.Performed);
            jumpCount++;
            rb.velocity = new Vector2((moveInput.x * moveSpeed), jumpForce);
            Debug.Log("phase");
        } 
        else if(ctx.phase == InputActionPhase.Waiting && isGrounded) {
            Debug.Log("fezfze");
            // jumpCount = 0;
        }
            // animator.SetBool("IsJumping", ctx.phase == InputActionPhase.Performed);
    }

    private void Flip()
    {
        if (moveInput.x > 0 && !isFacingRight || moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, listGroundLayers);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
