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

    private void Awake() {
        animator = GetComponentInChildren<Animator>();
    }

    private void Update() {
        animator.SetFloat(AnimationStrings.velocityY, rb.velocity.y);
        animator.SetBool(AnimationStrings.isGrounded, isGrounded);
    }

    private void FixedUpdate()
    {
        isFalling = IsFalling();
        isGrounded = IsGrounded();

        if (!isGrounded && !isGroundPounding)
        {
            Vector3 vel = rb.velocity;
            vel.y -= 10.5f * Time.deltaTime;
            rb.velocity = vel;
        }
    }

    public bool IsFalling()
    {
        return rb.velocity.y < 0;
    }

    public bool IsGrounded()
    {
        return Physics.OverlapSphere(groundCheck.position, playerData.groundCheckRadius, listGroundLayers).Length > 0;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            if (Time.time - lastGroundPoundCooldown < playerData.groundPoundCooldown)
            {
                return;
            }

            if (!isGrounded)
            {
                StopAndSpin();
            }

            if (isGrounded)
            {
                rb.velocity = new Vector3(rb.velocity.x, playerData.jumpForce, rb.velocity.z);
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
        rb.AddForce(Vector3.down * playerData.dropForce, ForceMode.Impulse);
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

        for (var i = 0; i <= nbColliders; i += 2)
        {
            // bottom left
            var val = Mathf.Lerp(0, -Mathf.PI / 2, (float)i / nbColliders);

            // top left
            // var val = Mathf.Lerp(0, (1 * Mathf.PI) / 2, (float) i / length);

            // top right
            // var val = Mathf.Lerp(Mathf.PI, (1 * Mathf.PI) / 2, (float) i / length);

            // bottom right
            // var val = Mathf.Lerp(Mathf.PI, (3 * Mathf.PI) / 2, (float) i / length);

            // var val = Mathf.Lerp(Mathf.PI, 2 * Mathf.PI, (float)i / length);

            var vertical = Mathf.Sin(val);
            var horizontal = Mathf.Cos(val);

            var spawnDir = new Vector3(horizontal, 0, vertical);
            var spawnPos = pos + spawnDir * radius;

            GameObject newSurrounderObject = Instantiate(playerData.waveEffect, spawnPos, Quaternion.identity);

            newSurrounderObject.transform.LookAt(pos);
            newSurrounderObject.transform.RotateAround(
                newSurrounderObject.transform.position,
                newSurrounderObject.transform.up,
                90
            );
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, playerData.groundCheckRadius);
        }
    }
}
