using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ChargeAttack : MonoBehaviour
{
    private RaycastHit2D target;
    private Animator animator;
    private new Collider2D collider;

    private Rigidbody2D rb;

    [SerializeField]
    private LayerMask targetLayerMask;

    int distance = 15;
    int dashDistance = 10;

    private bool isCharging = false;

    [SerializeField]
    private bool hasTouchedSomething = false;

    [SerializeField]
    private bool canCharge = true;

    [field: SerializeField]
    public bool isFacingRight { get; private set; } = false;

    [SerializeField]
    private UnityEvent onBegin, onDone;

    [SerializeField]
    private EnemyData enemyData;

    private EnemyFlying enemyFlying;

    private Knockback knockback;


    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        enemyFlying = GetComponent<EnemyFlying>();
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
           distance * new Vector3(1, 0.5f, 0),
        //    new Vector2(collider.bounds.size.x * distance, collider.bounds.size.y * distance),
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
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }
    private IEnumerator Dash(Vector3 targetPos)
    {
        canCharge = false;
        onBegin?.Invoke();
        rb.velocity = -(targetPos - transform.position).normalized * 5;
        animator.SetBool(AnimationStrings.attack, true);
        yield return Helpers.GetWait(0.75f);
        yield return null;
        isCharging = true;
        rb.velocity = (targetPos - transform.position).normalized * 25;
        yield return new WaitUntil(() => {
            return (
                rb.position.x > targetPos.x && isFacingRight || 
                rb.position.x < targetPos.x && !isFacingRight || 
                hasTouchedSomething == true
            );
        });
        rb.velocity = Vector2.zero;
        hasTouchedSomething = false;
        isCharging = false;
        animator.SetBool(AnimationStrings.attack, false);
        enemyFlying.SetStartingPosition(rb.position);
        onDone?.Invoke();
        yield return Helpers.GetWait(3.75f);
        canCharge = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isCharging)
        {
            hasTouchedSomething = true;
 
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
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(transform.position, distance * new Vector3(1, 0.5f, 0));
        }
    }
}
