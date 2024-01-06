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
    private bool isDashing = false;

    [SerializeField]
    private StringEventChannel countdownEvent;

    [SerializeField]
    private VectorEventChannel rbVelocityEventChannel;

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private CameraShakeTypeValue dashCameraShake;

    private float originalGravity;

    private List<int> listLayers = new List<int>();

    private string layer;

    private void Awake()
    {
        dashTrailRenderer = GetComponent<DashTrailRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();
        bc2d = GetComponentInParent<BoxCollider2D>();

        layer = LayerMask.LayerToName(gameObject.layer);
        originalGravity = rb.gravityScale;
        originalLayerName = LayerMask.LayerToName(gameObject.layer);
        listLayers = Helpers.GetLayersIndexFromLayerMask(listDashableLayers);

        Helpers.DisableCollisions(LayerMask.LayerToName(gameObject.layer), listLayers, false);
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
        if (isDashing)
        {
            InflictDamage();
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
            if(item.TryGetComponent(out Animator animator)) {
                Debug.Log(animator.GetBehaviour<MechaGuard>());
            }
            if (item.TryGetComponent(out IGuardable iGuardable))
            {
                if (iGuardable.isGuarding && item.transform.right.x != transform.right.x)
                {
                    DashEnd();
                    Knockback knockback = GetComponentInParent<Knockback>();
                    knockback.Apply(item.gameObject, 950);

                    return;
                }
            }

            if (item.gameObject != gameObject && item.TryGetComponent(out IDamageable iDamageable))
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
        Helpers.DisableCollisions(layer, listLayers, true);
        isDashing = true;
        canDash = false;
        rb.gravityScale = 0f;
        // gameObject.layer = LayerMask.NameToLayer("AttackArea");

        // // Time.timeScale = 0.5f;
        playerCanMove.CurrentValue = false;
        // onCinemachineShake.Raise(dashCameraShake);

        float speedFactor = Mathf.Abs(rb.velocity.x) > 0 ? 1.25f : 1;
        rb.velocity = new Vector2(transform.right.normalized.x * playerData.dashVelocity * speedFactor, 0);
        dashTrailRenderer.emit = true;
        rbVelocityEventChannel.Raise(rb.velocity);
        yield return Helpers.GetWait(0.35f);
        DashEnd();
        StartCoroutine(Countdown());
    }

    private void DashEnd()
    {
        rb.gravityScale = originalGravity;
        isDashing = false;
        // gameObject.layer = LayerMask.NameToLayer(originalLayerName);
        dashTrailRenderer.emit = false;
        rb.velocity = Vector2.zero;
        playerCanMove.CurrentValue = true;
        Helpers.DisableCollisions(layer, listLayers, false);
    }

    public IEnumerator Countdown()
    {
        int start = playerData.dashCooldown;
        while (start > 0)
        {
            countdownEvent.Raise(start.ToString());
            start--;
            yield return Helpers.GetWait(1);
        }
        countdownEvent.Raise(start.ToString());

        canDash = true;
    }
}
