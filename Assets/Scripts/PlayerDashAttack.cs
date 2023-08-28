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
    private BoolValue playerIsDashing;

    private string originalLayerName;

    [SerializeField]
    private PlayerHealth playerHealth;

    private BoxCollider2D bc2d;

    private float originalGravity;

    private void Awake()
    {
        dashTrailRenderer = GetComponent<DashTrailRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();
        bc2d = GetComponentInParent<BoxCollider2D>();
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        originalGravity = rb.gravityScale;
        originalLayerName = LayerMask.LayerToName(gameObject.layer);
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (playerIsDashing.CurrentValue)
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
            if (item.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage(playerData.dashDamage);
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bc2d.bounds.center, bc2d.bounds.size);
    }

    public IEnumerator Dash()
    {
        dashTrailRenderer.emit = true;
        // // canDash = false;
        rb.gravityScale = 0f;
        gameObject.layer = LayerMask.NameToLayer("AttackArea");

        // // Time.timeScale = 0.5f;
        playerIsDashing.CurrentValue = true;
        DisableCollisions(true);
        playerHealth.TakeDamage(1);
        // rb.AddForce(
        //     new Vector2(transform.right.normalized.x * playerData.dashVelocity, 0),
        //     ForceMode2D.Impulse
        // );
        rb.velocity = new Vector2(transform.right.normalized.x * playerData.dashVelocity, 0);
        yield return new WaitForSecondsRealtime(0.25f);
        rb.gravityScale = originalGravity;

        gameObject.layer = LayerMask.NameToLayer(originalLayerName);
        dashTrailRenderer.emit = false;
        rb.velocity = Vector2.zero;
        DisableCollisions(false);
        playerIsDashing.CurrentValue = false;
        // yield return new WaitForSecondsRealtime(0.5f);
        // canDash = true;
    }
}
