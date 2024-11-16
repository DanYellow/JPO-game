using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private bool isGrounded = false;

    private bool isFalling = false;

    private bool isGroundPounding = false;

    private float lastGroundPoundCooldown = 0;

    [SerializeField]
    private LayerMask listGroundLayers;

    [SerializeField]
    private Transform groundCheck;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private Animator animator;

    private ObjectPooling pool;

    private PlayerInput playerInput;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        pool = GetComponent<ObjectPooling>();

        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultActionMap = playerData.id.ToString();
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

    private void StopAndSpin()
    {
        isGroundPounding = true;
        lastGroundPoundCooldown = Time.time;

        StartCoroutine(DropAndPound());
    }

    private IEnumerator DropAndPound()
    {
        animator.SetTrigger(AnimationStrings.doubleJump);
        rb.constraints = RigidbodyConstraints.FreezePosition;
        yield return Helpers.GetWait(0.15f);
        rb.constraints = RigidbodyConstraints.None;
        rb.AddForce(Vector3.down * playerData.root.dropForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (isGroundPounding)
        {
            CreateShockwave(other.contacts[0].point);
        }
        isGroundPounding = false;
    }

    private void CreateShockwave(Vector3 pos)
    {
        int nbColliders = 4;
        float radius = 2;

        for (int i = 0; i <= nbColliders; i += 2)
        {
            float val = Mathf.Lerp(0, -Mathf.PI / 2, (float)i / nbColliders);

            switch (playerData.id)
            {
                case PlayerID.Player2:
                    val = Mathf.Lerp(Mathf.PI, 3 * Mathf.PI / 2, (float)i / nbColliders);
                    break;
                case PlayerID.Player3:
                    val = Mathf.Lerp(Mathf.PI / 2, Mathf.PI, (float)i / nbColliders);
                    break;
                case PlayerID.Player4:
                    val = Mathf.Lerp(0, Mathf.PI / 2, (float)i / nbColliders);
                    break;
                default:
                    break;
            }

            var vertical = Mathf.Sin(val);
            var horizontal = Mathf.Cos(val);

            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = pos + spawnDir * radius;

            ObjectPooled waveEffect = pool.Get("WaveAttack");
            if (waveEffect == null)
            {
                return;
            }

            waveEffect.gameObject.layer = LayerMask.NameToLayer($"WaveEffect{playerData.id.ToString()}");
            waveEffect.gameObject.transform.SetPositionAndRotation(spawnPos, Quaternion.identity);
            waveEffect.gameObject.transform.LookAt(pos);
            waveEffect.gameObject.transform.RotateAround(
                waveEffect.gameObject.transform.position,
                waveEffect.gameObject.transform.up,
                90
            );
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, playerData.root.groundCheckRadius);
        }
    }

    public void DisableControls() {
        playerInput.defaultActionMap = "PlayerDead";
    }
}
