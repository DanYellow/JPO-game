using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashAttack : MonoBehaviour
{
    private DashTrailRenderer dashTrailRenderer;
    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask listDashableLayers;

    [SerializeField]
    private PlayerStatsValue playerData;

    [SerializeField]
    private BoolValue playerCanMove;

    private string originalLayerName;

    private BoxCollider2D bc2d;

    private bool canDash = true;

    [SerializeField]
    private StringEventChannel countdownEvent;

    [SerializeField]
    private VectorEventChannel rbVelocityEventChannel;

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private CameraShakeTypeValue dashCameraShake;

    private float originalGravity;

    private void Awake()
    {
        dashTrailRenderer = GetComponent<DashTrailRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();
        bc2d = GetComponentInParent<BoxCollider2D>();
    }

    private void Start()
    {
        originalGravity = rb.gravityScale;
        originalLayerName = LayerMask.LayerToName(gameObject.layer);
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public void OnDashCancel(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed && !canDash)
        {
            rb.velocity = Vector2.zero;
            // rbVelocityEventChannel.Raise(rb.velocity);
        }
    }

    private void FixedUpdate()
    {
        if (!playerCanMove.CurrentValue && !canDash)
        {
            InflictDamage();
        }
    }

    private void DisableCollisions(bool enabled)
    {
        string[] layers = new string[] { "Enemy", "Props", "Traps" };
        foreach (var layer in layers)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer(layer), enabled);
        }
    }

    private void InflictDamage()
    {
        Collider2D[] hits = Physics2D.OverlapBoxAll(
           bc2d.bounds.center,
           bc2d.bounds.size,
            0,
            listDashableLayers
        );

        foreach (var item in hits)
        {
            if (item.TryGetComponent(out IGuardable iGuardable))
            {
                if (iGuardable.isGuarding && item.transform.right.x != transform.right.x) return;
            }

            if (item.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage(playerData.dashDamage);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.yellow;
        // Gizmos.DrawWireCube(bc2d.bounds.center, bc2d.bounds.size);
    }

    private IEnumerator Dash()
    {
        canDash = false;
        rb.gravityScale = 0f;
        gameObject.layer = LayerMask.NameToLayer("AttackArea");

        // // Time.timeScale = 0.5f;
        playerCanMove.CurrentValue = false;
        DisableCollisions(true);
        // onCinemachineShake.Raise(dashCameraShake);

        float speedFactor = Mathf.Abs(rb.velocity.x) > 0 ? 1.25f : 1;
        rb.velocity = new Vector2(transform.right.normalized.x * playerData.dashVelocity * speedFactor, 0);
        dashTrailRenderer.emit = true;
        rbVelocityEventChannel.Raise(rb.velocity);
        yield return new WaitForSecondsRealtime(0.35f);
        DashEnd();
        StartCoroutine(Countdown());
    }

    private void DashEnd()
    {
        rb.gravityScale = originalGravity;

        gameObject.layer = LayerMask.NameToLayer(originalLayerName);
        dashTrailRenderer.emit = false;
        rb.velocity = Vector2.zero;
        DisableCollisions(false);
        playerCanMove.CurrentValue = true;
    }

    public IEnumerator Countdown()
    {
        int start = playerData.dashCooldown;
        while (start > 0)
        {
            countdownEvent.Raise(start.ToString());
            start--;
            yield return new WaitForSeconds(1);
        }
        countdownEvent.Raise(start.ToString());

        canDash = true;
    }
}
