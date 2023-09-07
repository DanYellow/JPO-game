using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    private RaycastHit2D target;
    private Animator animator;
    private new Collider2D collider;
    private Collider2D targetCollider;

    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask targetLayerMask;

    int distance = 6;

    bool isTeleporting = false;

    private bool isDashing = false;

    [field: SerializeField]
    public bool isFacingRight { get; private set; } = false;

    private Vector2 startingPosition;

    private Knockback knockback;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();

        startingPosition = transform.position;
    }

    private void FixedUpdate()
    {
        target = Physics2D.BoxCast(
           collider.bounds.center,
           collider.bounds.size * distance,
           0,
           Vector2.zero,
           0,
           targetLayerMask
        );

        if (target)
        {
            if (
                target.collider.transform.position.x > transform.position.x && !isFacingRight ||
                target.collider.transform.position.x < transform.position.x && isFacingRight
            )
            {
                Flip();
            }

            if(!isDashing && Vector2.Distance(target.collider.transform.position, transform.position) < 4) {
                StartCoroutine(Dash());
            }
        } else if(Vector2.Distance(startingPosition, transform.position) > 15 && !isTeleporting && !isDashing) {
            StartCoroutine(ReturnToStartPoint());
        }

        targetCollider = Physics2D.OverlapCircle(
            collider.bounds.center,
            1.75f,
            targetLayerMask
        );
        if(targetCollider) {
            animator.SetTrigger(AnimationStrings.attack);
        }

        #region DrawLine
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y - distance - (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y - distance - (collider.bounds.size.y / 2)
            ),
            Color.magenta
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y + distance + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y + distance + (collider.bounds.size.y / 2)
            ),
            Color.green
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y + distance + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x - distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y - distance - (collider.bounds.size.y / 2)
            ),
            Color.red
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x + distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y + distance + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y - distance - (collider.bounds.size.y / 2)
            ),
            Color.yellow
        );
        #endregion

    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    private IEnumerator Dash()
    {
        isDashing = true;
        rb.velocity = new Vector2(transform.right.normalized.x * 15, rb.velocity.y);
        yield return new WaitForSecondsRealtime(0.35f);
        isDashing = false;
        rb.velocity = Vector2.zero;
    }

    IEnumerator ReturnToStartPoint()
    {
        isTeleporting = true;
        animator.SetTrigger("TeleportIn");
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        transform.position = startingPosition;
        animator.SetTrigger("TeleportOut");
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        isTeleporting = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(isDashing) {
            print(other.transform.name);
        }
    }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (animator.GetTrig)
    //     {            
    //         if(other.gameObject.TryGetComponent(out Knockback knockback)) {
    //             knockback.Apply(gameObject, 10);
    //         }

    //         if(other.gameObject.TryGetComponent(out IDamageable iDamageable)) {
    //            iDamageable.TakeDamage(enemyData.damage);
    //         }
    //     }
    // }

    private void OnDrawGizmos() {
        if(collider) {
            Gizmos.DrawWireSphere(collider.transform.position, 1.75f);
        }
    }
}
