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

    [SerializeField]
    private bool canCharge = true;

    [field: SerializeField]
    public bool isFacingRight { get; private set; } = false;

    [SerializeField]
    private UnityEvent onBegin, onDone;

    [SerializeField]
    private EnemyData enemyData;


    private Knockback knockback;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }

    private void Update()
    {
        animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));
    }

    private void FixedUpdate()
    {
        target = Physics2D.BoxCast(
           collider.bounds.center,
           new Vector2(collider.bounds.size.x * distance, collider.bounds.size.y),
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

        DrawRectDebug(distance, distance);
        DrawRectDebug(dashDistance);
    }

    private void DrawRectDebug(float width, float height = 0)
    {
        #region DrawLine
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - width - (collider.bounds.size.x / 2),
                collider.bounds.center.y - height - (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + width + (collider.bounds.size.x / 2),
                collider.bounds.center.y - height - (collider.bounds.size.y / 2)
            ),
            Color.magenta
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - width - (collider.bounds.size.x / 2),
                collider.bounds.center.y + height + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + width + (collider.bounds.size.x / 2),
                collider.bounds.center.y + height + (collider.bounds.size.y / 2)
            ),
            Color.green
        );

        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x - width - (collider.bounds.size.x / 2),
                collider.bounds.center.y + height + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x - width - (collider.bounds.size.x / 2),
                collider.bounds.center.y - height - (collider.bounds.size.y / 2)
            ),
            Color.red
        );
        Debug.DrawLine(
            new Vector2(
                collider.bounds.center.x + width + (collider.bounds.size.x / 2),
                collider.bounds.center.y + height + (collider.bounds.size.y / 2)
            ),
            new Vector2(
                collider.bounds.center.x + width + (collider.bounds.size.x / 2),
                collider.bounds.center.y - height - (collider.bounds.size.y / 2)
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
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);

        isCharging = true;
        canCharge = false;
        rb.velocity = (targetPos - transform.position).normalized * 12;
        yield return Helpers.GetWait(0.75f);
        isCharging = false;
        rb.velocity = Vector2.zero;
        animator.SetBool(AnimationStrings.attack, false);
        yield return Helpers.GetWait(2.75f);
        onDone?.Invoke();
        canCharge = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isCharging)
        {
            rb.velocity = Vector2.zero;
            knockback.Apply(other.gameObject, 1500);
            if (other.gameObject.TryGetComponent(out Knockback _knockback))
            {
                _knockback.Apply(gameObject, enemyData.knockbackForce);
            }

            if (other.gameObject.CompareTag("Player"))
            {
                IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
                iDamageable.TakeDamage(enemyData.damage);
            }
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
