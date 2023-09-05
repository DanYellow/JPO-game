using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

// https://www.youtube.com/watch?v=xx1oKVTU_gM

public class PlayerMovements : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector3 moveInput = Vector3.zero;

    [SerializeField, ReadOnlyInspector]
    private bool isFacingRight = true;

    [Space(15), Tooltip("Position checks"), SerializeField]
    private bool isGrounded;

    [SerializeField]
    private LayerMask listGroundLayers;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius;

    private Vector2 nextPosition;

    private BoxCollider2D bc2d;
    private BoxCollider2D bc2dChild;

    [SerializeField]
    private BoolValue playerCanMove;


    [SerializeField]
    private PlayerStatsValue playerData;

    private ParticleSystem dust;

    [SerializeField]
    private int jumpCount = 0;

    [SerializeField]
    private bool showOnStart = true;
    private bool isSliding = false;

    [SerializeField]
    private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

     [SerializeField]
    private LayerMask listSlidingLayers;

    private List<int> listLayers = new List<int>();
    private string layer;

    [Header("Events")]
    [SerializeField]
    private VectorEventChannel rbVelocityEventChannel;

    [SerializeField]
    private VectorEventChannel playerPositionEventChannel;

    [SerializeField]
    private BoolEventChannel playerCrouchEventChannel;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        dust = GetComponentInChildren<ParticleSystem>();
        bc2d = GetComponent<BoxCollider2D>();
        bc2dChild = GetComponentsInChildren<BoxCollider2D>()[1];
        gameObject.SetActive(showOnStart);

        layer = LayerMask.LayerToName(gameObject.layer);
        listLayers = Helpers.GetLayersIndexFromLayerMask(listSlidingLayers);
    }

    private void Start()
    {
        playerCanMove.CurrentValue = true;
    }

    void Update()
    {
        Flip();

        if (!playerCanMove.CurrentValue)
        {
            return;
        }
        playerPositionEventChannel.Raise(transform.position);
        rbVelocityEventChannel.Raise(rb.velocity);


        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // if (isGrounded && rb.velocity.y < 0.1f)
        // {
        //     jumpCount = 0;
        // }

        if (rb.velocity.y >= 0)
        {
            rb.gravityScale = playerData.gravityScaleGrounded;
        }
        else if (rb.velocity.y < -0.1f)
        {
            rb.gravityScale = playerData.gravityScaleFalling;
        }

        if (Mathf.Abs(rb.velocity.x) > 0.1f && isGrounded && rb.velocity.y < 0.1f)
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
        if (playerCanMove.CurrentValue)
        {
            nextPosition = new Vector2(moveInput.x * playerData.moveSpeed, rb.velocity.y);
            rb.velocity = nextPosition;
        }
        else
        {
            // rb.velocity = Vector2.zero;
        }
        isGrounded = IsGrounded();
    }

    public void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {

            playerCrouchEventChannel.OnEventRaised(true);
            
            // if(rb.velocity.x > 1 && !isSliding) {
            //     isSliding = true;
            //     rb.AddForce(transform.right.normalized * 10);
            //     StartCoroutine(StopSlide());
            // } 
            // else
            // {
            //     // rb.velocity = Vector2.zero;

            // }
            playerCanMove.CurrentValue = false;
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            isSliding = false;
            DisableCollisions(false);
            playerCrouchEventChannel.OnEventRaised(false);
            playerCanMove.CurrentValue = true;
        }
        else if (ctx.phase == InputActionPhase.Started)
        {
            playerCrouchEventChannel.OnEventRaised(true);
            if (Mathf.Abs(rb.velocity.x) > 1 && !isSliding)
            {
                DisableCollisions(true);
                
                isSliding = true;
                rb.AddForce(transform.right.normalized * 5);
                StartCoroutine(StopSlide());
            }
        }
        StartCoroutine(CrouchProxy());
    }

    private IEnumerator StopSlide()
    {
        yield return Helpers.GetWait(0.45f);
        rb.velocity = Vector2.zero;
        DisableCollisions(false);
    }

    IEnumerator CrouchProxy()
    {
        yield return null;

        bc2d.offset = new Vector2(
            bc2d.offset.x,
            bc2dChild.offset.y
        );
        bc2d.size = bc2dChild.size;
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
            new Vector3(bc2d.bounds.size.x * 0.95f, 0.5f, 0),
            0,
            listGroundLayers
        );
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, listGroundLayers);
    }

    private void DisableCollisions(bool enabled)
    {
        foreach (var layerIndex in listLayers)
        {
            //  Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), layerIndex, enabled);
            Physics2D.IgnoreLayerCollision(
                LayerMask.NameToLayer(layer), 
                layerIndex, 
                enabled
            );
        }
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (
            ctx.phase == InputActionPhase.Performed &&
            // jumpCount < playerData.maxJumpCount &&
            playerCanMove.CurrentValue &&
            coyoteTimeCounter > 0f
        )
        {
            jumpCount++;
            // float jumpForce = Mathf.Sqrt(playerData.jumpForce * (Physics2D.gravity.y * rb.gravityScale) * -2) * rb.mass;
            // rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            rb.velocity = new Vector2(moveInput.x * playerData.moveSpeed, playerData.jumpForce);
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            coyoteTimeCounter = 0f;
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
            Gizmos.DrawWireCube(groundCheck.position - new Vector3(bc2d.offset.x, 0, 0), new Vector3(bc2d.bounds.size.x * 0.95f, 0.5f, 0));
            // Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(bc2d.bounds.center, bc2d.bounds.size);
    }
}
