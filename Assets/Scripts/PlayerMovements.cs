using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovements : MonoBehaviour, IPushable
{
    private Rigidbody2D rb;

    [SerializeField]
    Animator animator;

    private Vector3 moveInput = Vector3.zero;

    [SerializeField, ReadOnlyInspector]
    private bool isFacingRight = true;

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

    private ContactPoint2D[] listContacts = new ContactPoint2D[1];


    [Space(15)]

    [Tooltip("Position checks")]
    private bool isGrounded;
    public LayerMask listGroundLayers;
    public Transform groundCheck;
    public float groundCheckRadius;

    private float moveSpeed;

    [Header("Jump system")]
    private int jumpCount = 0;
    private int maxJumpCount;

    private bool isHitted = false;

    private float fallThreshold;

    private Vector2 nextPosition;

    private float speedFactor;

    [SerializeField]
    private PlayerInput playerInput;

    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("playerStatsValue")]
    private PlayerStatsValue playerData;

    public LayerMask waterLayer;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // playerInput = GetComponent<PlayerInput>();

        isHurtVoidEventChannel.OnEventRaised += OnHurt;

        moveSpeed = playerData.moveSpeed;
        maxJumpCount = playerData.maxJumpCount;
        fallThreshold = playerData.fallThreshold;
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
        speedFactor = isInWater ? playerData.waterSpeedFactor : playerData.speedFactor;

        Flip();
    }

    private void FixedUpdate()
    {
        if (!isHitted)
        {
            nextPosition = new Vector2((moveInput.x * moveSpeed) * speedFactor, rb.velocity.y);
            if (moveInput.y <= -0.25f)
            {
                nextPosition.x = 0;
            }
            rb.velocity = nextPosition;
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
            rb.velocity = new Vector2((moveInput.x * moveSpeed) * speedFactor, playerData.jumpForce * speedFactor);
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
        yield return new WaitForSeconds(0.25f);
        isHitted = false;
    }

    public void HitDirection(ContactPoint2D contactPoint)
    {
        Vector2 pushBackVector = new Vector2(contactPoint.normal.x, 0) * -1;
        rb.AddForce(pushBackVector * playerData.knockbackForce, ForceMode2D.Impulse);
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
