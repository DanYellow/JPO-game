using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

// https://www.youtube.com/watch?v=xx1oKVTU_gM

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;

    private Animator animator;

    private Vector3 moveInput = Vector3.zero;

    [SerializeField, ReadOnlyInspector]
    private bool isFacingRight = true;

    [Header("Events")]
    [SerializeField]
    private VectorEventChannel vectorEventChannel;

    [Space(15), Tooltip("Position checks")]
    private bool isGrounded;
    public LayerMask listGroundLayers;
    public Transform groundCheck;
    public float groundCheckRadius;
    private float moveSpeed;

    private bool isHitted = false;
    private float fallThreshold;
    private Vector2 nextPosition;

    [SerializeField]
    private PlayerInput playerInput;
    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("playerStatsValue")]
    private PlayerStatsValue playerData;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        moveSpeed = playerData.moveSpeed;
    }

    void Update()
    {
        vectorEventChannel.Raise(moveInput);

        Flip();
    }

    private void FixedUpdate()
    {
        if (!isHitted)
        {
            nextPosition = new Vector2(moveInput.x * moveSpeed, rb.velocity.y);
            if (moveInput.y <= -0.25f)
            {
                nextPosition.x = 0;
            }
            rb.velocity = nextPosition;
        }

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

    IEnumerator OnHurtProxy()
    {
        isHitted = true;
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
}
