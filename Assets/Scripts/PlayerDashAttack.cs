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

    private float originalGravity;

    private void Awake()
    {
        dashTrailRenderer = GetComponent<DashTrailRenderer>();
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Start()
    {
        originalGravity = rb.gravityScale;

        // print(LayerMask.GetMask(listDashableLayers));
    }

    private void Update()
    {
        if (playerIsDashing.CurrentValue)
        {
            print("dashiin");
            // rb.velocity = new Vector2(transform.right.normalized.x * playerData.dashVelocity, 0);
        }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            StartCoroutine(Dash());
        }
    }

    private void DisableCollisions(bool enabled)
    {
        string[] layers = new string[] { "Enemy", "Props" };
        foreach (var layer in layers)
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Default"), LayerMask.NameToLayer(layer), enabled);
        }
    }

    public IEnumerator Dash()
    {
        // onPlayerDash.Raise(true);
        dashTrailRenderer.emit = true;
        // // canDash = false;
        rb.gravityScale = 0f;
        // // Time.timeScale = 0.5f;
        playerIsDashing.CurrentValue = true;
        DisableCollisions(true);
        rb.velocity = new Vector2(transform.right.normalized.x * playerData.dashVelocity, rb.velocity.y);

        yield return new WaitForSecondsRealtime(0.5f);
        playerIsDashing.CurrentValue = false;
        rb.gravityScale = originalGravity;
        // isDashing = false;
        // Time.timeScale = 1f;
        // onPlayerDash.Raise(false);
        dashTrailRenderer.emit = false;
        DisableCollisions(false);
        // yield return new WaitForSecondsRealtime(0.5f);
        // canDash = true;
    }
}
