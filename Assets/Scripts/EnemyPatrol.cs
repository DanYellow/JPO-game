using UnityEngine;
using System.Collections;
using UnityEngine.Serialization;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private float maxMoveSpeed;

    public EnemyStatsValue enemyData;

    [SerializeField]
    private float currentMoveSpeed;

    private SpriteRenderer sr;

    public bool isFacingRight = false;

    private bool isIdle = true;

    private float idleTime;

    private Vector3 offset;

    private Vector3 lastKnownPosition = Vector3.zero;

    private bool isGrounded;
    public float groundCheckRadius = 0.25f;
    [SerializeField]
    private Vector3 additionnalGroundCheckOffset = Vector3.zero;
    private bool isFlipping = false;

    [Tooltip("Define how long the enemy will walk")]
    public float walkTime = 5f;

    [FormerlySerializedAs("layerMask")]
    public LayerMask obstacleLayersMask;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        enabled = false;
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        maxMoveSpeed = enemyData.moveSpeed * enemyData.accelerationRate;
        currentMoveSpeed = enemyData.moveSpeed;

        offset = new Vector3(sr.bounds.extents.x * (isFacingRight ? -1 : 1), sr.bounds.extents.y, 0);
    }

    private void Start()
    {
        idleTime = Mathf.Round(Random.Range(0, 3.5f));
        StartCoroutine(ChangeState());

        InvokeRepeating(nameof(UpdateLastKnownPosition), 3.0f, 3f);
    }

    void UpdateLastKnownPosition()
    {
        if (lastKnownPosition == transform.position && !isFlipping && (rb.velocity.y > -0.1f && rb.velocity.y < 0.1f))
        {
            StartCoroutine(Flip());
        }
        lastKnownPosition = transform.position;
    }

    private void Update()
    {
        if (isIdle)
        {
            Idle();
        }
        else
        {
            Move();
        }

        animator.SetFloat("MoveDirectionX", Mathf.Abs(rb.velocity.x));
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        Vector3 startCast = transform.position - new Vector3(offset.x, 0, 0);
        Vector3 endCast = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.75f;
        Debug.DrawLine(startCast, endCast, Color.green);

        RaycastHit2D hitObstacle = Physics2D.Linecast(startCast, endCast, obstacleLayersMask);

        if (hitObstacle.collider != null && isGrounded)
        {
            if (hitObstacle.distance < 0.05f && hitObstacle.collider.gameObject.layer == LayerMask.NameToLayer("Platforms"))
            {
                StartCoroutine(Flip());
            }
            else if (hitObstacle.collider.gameObject.CompareTag("Player"))
            {
                currentMoveSpeed = Mathf.Clamp(currentMoveSpeed * enemyData.accelerationRate, enemyData.moveSpeed, maxMoveSpeed);
            }
        }
        else
        {
            currentMoveSpeed = Mathf.Clamp(currentMoveSpeed / enemyData.accelerationRate, enemyData.moveSpeed, maxMoveSpeed);
        }

        if (!isGrounded)
        {
            StartCoroutine(Flip());
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position - (offset + additionnalGroundCheckOffset), groundCheckRadius, obstacleLayersMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - (offset + additionnalGroundCheckOffset), groundCheckRadius);
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            // Enemy will walk during X seconds...
            isIdle = false;
            yield return new WaitForSeconds(walkTime);

            // ...then wait during X seconds...
            isIdle = true;
            yield return new WaitForSeconds(idleTime);
        }
    }

    private void Idle()
    {
        rb.velocity = Vector2.zero;
    }

    private void Move()
    {
        if (isFacingRight)
        {
            rb.velocity = new Vector2(currentMoveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-currentMoveSpeed, rb.velocity.y);
        }
    }

    IEnumerator Flip()
    {
        offset.x *= -1;
        additionnalGroundCheckOffset.x *= -1;
        isFlipping = true;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        lastKnownPosition = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        isFlipping = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        // We stop the enemy when is not visible or else
        // it might continue to run but whoen be able to change direction
        Idle();
        enabled = false;
    }
}