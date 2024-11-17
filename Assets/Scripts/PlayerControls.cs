using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private bool isGrounded = false;

    private bool isFalling = false;

    private bool isGroundPounding = false;

    private float lastGroundPoundCooldown = 0;

    [SerializeField]
    private LayerMask listGroundLayers;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private UnityEvent<GameObject> onGroundPound;
    

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private VoidEventChannel onGameEndEvent;

    [SerializeField]
    private PlayerIDEventChannel onPlayerWinsEvent;

    private Animator animator;

    private PlayerInput playerInput;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();

        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultActionMap = playerData.id.ToString();
    }

    private void OnEnable()
    {
        onGameEndEvent.OnEventRaised += OnGameEnd;
    }

    private void Update()
    {
        animator.SetFloat(AnimationStrings.velocityY, rb.linearVelocity.y);
        animator.SetBool(AnimationStrings.isGrounded, isGrounded);
    }

    private void FixedUpdate()
    {
        isFalling = IsFalling();
        isGrounded = IsGrounded();

        if (!isGrounded && !isGroundPounding)
        {
            Vector3 vel = rb.linearVelocity;
            vel.y -= 10.5f * Time.deltaTime;
            rb.linearVelocity = vel;
        }
    }

    public bool IsFalling()
    {
        return rb.linearVelocity.y < 0;
    }

    public bool IsGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, playerData.root.groundCheckRadius, listGroundLayers).Length > 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // context.ReadValue<bool>();
        // context.action.triggered;

        if (context.phase == InputActionPhase.Performed)
        {
            if (Time.time - lastGroundPoundCooldown < playerData.root.groundPoundCooldown)
            {
                return;
            }

            if (!isGrounded)
            {
                StopAndSpin();
            }

            if (isGrounded)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, playerData.root.jumpForce, rb.linearVelocity.z);
            }
        }
    }

    private void OnGameEnd()
    {
        if (playerData.nbLives > 0)
        {
            DisableControls();
            onPlayerWinsEvent.Raise(playerData.id);
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
        animator.SetTrigger(AnimationStrings.doubleJump);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        yield return Helpers.GetWait(0.15f);
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.AddForce(Vector3.down * playerData.root.dropForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isGroundPounding)
        {
            onGroundPound.Invoke(gameObject);
        }
        isGroundPounding = false;
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, playerData.root.groundCheckRadius);
        }
    }

    public void DisableControls()
    {
        // if (gameObject.activeInHierarchy)
        // {
        //     playerInput.SwitchCurrentActionMap("PlayerDead");
        // }
    }

    private void OnDisable()
    {
        onGameEndEvent.OnEventRaised += OnGameEnd;
    }
}
