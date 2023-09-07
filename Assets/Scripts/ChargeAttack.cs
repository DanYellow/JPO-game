using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChargeAttack : MonoBehaviour
{
    private RaycastHit2D target;
    private Animator animator;
    private new Collider2D collider;
    private Collider2D targetCollider;

    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask targetLayerMask;

    int distance = 15;
    int dashDistance = 10;

    private bool isCharging = false;
    private bool canCharge = true;

    [field: SerializeField]
    public bool isFacingRight { get; private set; } = false;

    [SerializeField]
    private UnityEvent onBegin, onDone;


    private Knockback knockback;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
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
                (target.collider.transform.position.x > transform.position.x && !isFacingRight ||
                target.collider.transform.position.x < transform.position.x && isFacingRight) &&
                !isCharging
            )
            {
                Flip();
            }

            if (canCharge && Vector2.Distance(target.collider.bounds.min, transform.position) <= dashDistance)
            {

                StartCoroutine(Dash(target.collider.transform.position));
            }
        }

        // targetCollider = Physics2D.OverlapCircle(
        //     collider.bounds.center,
        //     1.75f,
        //     targetLayerMask
        // );
        // if (targetCollider)
        // {
        //     animator.SetTrigger(AnimationStrings.attack);
        // }

        DrawRectDebug(distance);
        DrawRectDebug(dashDistance);
    }

    private void DrawRectDebug(float _distance)
    {
        #region DrawLine
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - _distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y - _distance - (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + _distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y - _distance - (collider.bounds.size.y / 2)
            ),
            Color.magenta
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - _distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y + _distance + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + _distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y + _distance + (collider.bounds.size.y / 2)
            ),
            Color.green
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - _distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y + _distance + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x - _distance - (collider.bounds.size.x / 2),
                collider.bounds.center.y - _distance - (collider.bounds.size.y / 2)
            ),
            Color.red
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x + _distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y + _distance + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + _distance + (collider.bounds.size.x / 2),
                collider.bounds.center.y - _distance - (collider.bounds.size.y / 2)
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
    private IEnumerator Dash(Vector3 targetPos)
    {
        onBegin?.Invoke();
        animator.SetBool(AnimationStrings.attack, true);

        isCharging = true;
        canCharge = false;
        rb.velocity = (targetPos - transform.position).normalized * 20;
        yield return Helpers.GetWait(0.75f);
        onDone?.Invoke();
        isCharging = false;
        rb.velocity = Vector2.zero;
        yield return Helpers.GetWait(2.25f);
        canCharge = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isCharging)
        {
            rb.velocity = Vector2.zero;
            knockback.Apply(other.gameObject, 1500);
            animator.SetBool(AnimationStrings.attack, false);
            if (other.gameObject.TryGetComponent(out Knockback _knockback))
            {
                _knockback.Apply(gameObject, 100);
            }

            // if(other.gameObject.TryGetComponent(out IDamageable iDamageable)) {
            //    iDamageable.TakeDamage(enemyData.damage);
            // }
        }
    }

    private void OnDrawGizmos()
    {
        if (collider)
        {
            Gizmos.DrawWireSphere(collider.transform.position, 1.75f);
        }
    }
}
