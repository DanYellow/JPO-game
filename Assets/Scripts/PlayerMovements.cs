using UnityEngine;
using UnityEngine.InputSystem;

// https://www.youtube.com/watch?v=xx1oKVTU_gM

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector3 moveInput = Vector3.zero;

    [SerializeField, ReadOnlyInspector]
    private bool isFacingRight = true;

    [Space(15), Tooltip("Position checks")]
    private bool isGrounded;
    public LayerMask listGroundLayers;
    public Transform groundCheck;
    public float groundCheckRadius;
    private float moveSpeed;
    private Vector2 nextPosition;

    [Header("Events")]
    [SerializeField]
    private VectorEventChannel vectorEventChannel;

    [SerializeField]
    private PlayerStatsValue playerData;

    private ParticleSystem dust;

    [SerializeField]
    private bool showOnStart = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dust = GetComponentInChildren<ParticleSystem>();

        moveSpeed = playerData.moveSpeed;
        gameObject.SetActive(showOnStart);
    }

    void Update()
    {
        vectorEventChannel.Raise(moveInput);

        Flip();

        if (Mathf.Abs(rb.velocity.x) > 0)
        {
            if(!dust.isEmitting) {
                dust.Play();
            }
        } else {
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
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, listGroundLayers);
    }

    void CreateDust()
    {
        // dust.Play();
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
