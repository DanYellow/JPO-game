using UnityEngine;
using UnityEngine.InputSystem;

// https://www.youtube.com/watch?v=xx1oKVTU_gM

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector3 moveInput = Vector3.zero;

    [SerializeField, ReadOnlyInspector]
    private bool isFacingRight = true;

    [Space(15), Tooltip("Position checks"), SerializeField]
    private bool isGrounded;
    public LayerMask listGroundLayers;
    public Transform groundCheck;
    public float groundCheckRadius;
    private float moveSpeed;
    private Vector2 nextPosition;

    private BoxCollider2D bc2d;


    [SerializeField]
    private PlayerStatsValue playerData;

    private ParticleSystem dust;

    [SerializeField]
    private int jumpCount = 0;

    [SerializeField]
    private bool showOnStart = true;

    [Header("Events")]
    [SerializeField]
    private VectorEventChannel vectorEventChannel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dust = GetComponentInChildren<ParticleSystem>();
        bc2d = GetComponent<BoxCollider2D>();

        moveSpeed = playerData.moveSpeed;
        gameObject.SetActive(showOnStart);
    }

    void Update()
    {
        vectorEventChannel.Raise(rb.velocity);

        Flip();

        if (isGrounded && rb.velocity.y < 0.1f)
        {
            jumpCount = 0;
        }

        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = playerData.gravityScaleGrounded;
        }
        else if (rb.velocity.y < -0.1f)
        {
            rb.gravityScale = playerData.gravityScaleFalling;
        }

        if (Mathf.Abs(rb.velocity.x) > 0 && isGrounded && rb.velocity.y < 0.1f)
        {
            if (!dust.isEmitting)
            {
                dust.Play();
            }
        }
        else
        {
            dust.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        }
    }

    private void FixedUpdate()
    {
        nextPosition = new Vector2(moveInput.x * playerData.moveSpeed, rb.velocity.y);
        rb.velocity = nextPosition;
        isGrounded = IsGrounded();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }

    private void Flip()
    {
        if (moveInput.x > 0 && !isFacingRight || moveInput.x < 0 && isFacingRight)
        {
            CreateDust();
            isFacingRight = !isFacingRight;
            transform.Rotate(0f, 180f, 0f);
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapBox(
           groundCheck.position - new Vector3(bc2d.offset.x, 0, 0),
            new Vector3(bc2d.bounds.size.x * 0.8f, 0.2f, 0),
            0,
            listGroundLayers
        );
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, listGroundLayers);
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (
            ctx.phase == InputActionPhase.Performed &&
            jumpCount < playerData.maxJumpCount
        )
        {
            jumpCount++;
            float jumpForce = Mathf.Sqrt(playerData.jumpForce * (Physics2D.gravity.y * rb.gravityScale) * -2) * rb.mass;
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            // rb.velocity = new Vector2(moveInput.x * playerData.moveSpeed, playerData.jumpForce);
        }
    }

    void CreateDust()
    {
        // dust.Play();
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            if (bc2d == null)
            {
                bc2d = GetComponent<BoxCollider2D>();
            }
            Gizmos.DrawWireCube(groundCheck.position - new Vector3(bc2d.offset.x, 0, 0), new Vector3(bc2d.bounds.size.x * 0.8f, 0.2f, 0));
            // Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bc2d.bounds.center, bc2d.bounds.size);
    }
}
