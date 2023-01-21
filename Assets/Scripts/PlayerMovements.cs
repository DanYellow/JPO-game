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
    private bool isGrounded;
    private bool isInWater;

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
    private BoolEventChannel isInWaterBoolEventChannel;

    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [Space(15)]

    [Tooltip("Position checks")]
    public LayerMask listGroundLayers;
    public Transform groundCheck;
    public float groundCheckRadius;

    private float moveSpeed;

    [Header("Jump system")]
    public int jumpCount = 0;
    private int maxJumpCount;
    private float jumpForce;

    private float backForce;

    private bool isHitted = false;

    private float fallThreshold;

    private float speedFactor;

    private PlayerInput playerInput;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    public LayerMask waterLayer;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();

        isHurtVoidEventChannel.OnEventRaised += OnHurt;

        moveSpeed = playerStatsValue.moveSpeed;
        jumpForce = playerStatsValue.jumpForce;
        maxJumpCount = playerStatsValue.maxJumpCount;
        backForce = playerStatsValue.knockbackForce;
        fallThreshold = playerStatsValue.fallThreshold;
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
        isInWaterBoolEventChannel.Raise(isInWater);
        speedFactor = isInWater ? playerStatsValue.waterSpeedFactor : playerStatsValue.speedFactor;

        Flip();
    }

    private void FixedUpdate()
    {
        if (!isHitted)
        {
            rb.velocity = new Vector2((moveInput.x * moveSpeed) * speedFactor, rb.velocity.y);
        }

        isGrounded = IsGrounded();
        isInWater = IsInWater();
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
            rb.velocity = new Vector2((moveInput.x * moveSpeed) * speedFactor, jumpForce * speedFactor);
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

    public bool IsInWater()
    {

        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, waterLayer);
    }

    private void OnHurt()
    {
        StartCoroutine(OnHurtProxy());
    }

    IEnumerator OnHurtProxy()
    {
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
