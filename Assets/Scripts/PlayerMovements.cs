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
    [SerializeField]
    private VectorEventChannel vectorEventChannel;
    [SerializeField]
    private BoolEventChannel jumpBoolEventChannel;
    [SerializeField]
    private BoolEventChannel isGroundedBoolEventChannel;
    [SerializeField]
    private BoolEventChannel fallingBoolEventChannel;

    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    // private UnityAction onHurtEvent;

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

    [Range(0, 500)]
    public float backForce;

    private bool isHitted = false;

    private float fallThreshold = -10f;

    PlayerInput playerInput;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        isHurtVoidEventChannel.OnEventRaised += OnHurt;
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded && rb.velocity.y < 0.1f)
        {
            jumpBoolEventChannel.Raise(playerInput.actions["Jump"].WasReleasedThisFrame());
            jumpCount = 0;
        }

        vectorEventChannel.Raise(moveInput);
        isGroundedBoolEventChannel.Raise(isGrounded);

        Flip();
    }

    private void FixedUpdate()
    {
        if (!isHitted)
        {
            rb.velocity = new Vector2((moveInput.x * moveSpeed), rb.velocity.y);
        }

        isGrounded = IsGrounded();

        if (rb.velocity.y < fallThreshold)
        {
            fallingBoolEventChannel.Raise(true);
        }
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (
            ctx.phase == InputActionPhase.Performed &&
            jumpCount < maxJumpCount &&
            moveInput.y > -0.5f
        )
        {
            jumpBoolEventChannel.Raise(ctx.phase == InputActionPhase.Performed);
            jumpCount++;
            rb.velocity = new Vector2((moveInput.x * moveSpeed), jumpForce);
        }
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

    private void OnHurt()
    {
       StartCoroutine(OnHurtProxy()); 
    }

    IEnumerator OnHurtProxy() {
        isHitted = true;
        int factor = isFacingRight ? -1 : 1;
        Vector2 pushBackVector = new Vector2(
            transform.position.normalized.x,
            0
        ) * factor;
        rb.AddForce(pushBackVector * backForce, ForceMode2D.Impulse);
        yield return new WaitForSeconds(0.25f);
        isHitted = false;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }

    private void OnDestroy()
    {
        isHurtVoidEventChannel.OnEventRaised -= OnHurt;
    }
}
