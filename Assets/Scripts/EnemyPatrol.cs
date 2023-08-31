
using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D bc;

    [SerializeField]
    private float walkSpeed;

    public bool isFacingRight = false;

    private bool isIdle = true;

    private float idleTime;

    [Tooltip("Define how long the enemy will walk"), SerializeField]
    private float walkTime = 5f;

    [SerializeField]
    private Vector2 offset = Vector2.zero;

    private Vector2 detectorPosition;

    private Vector3 lastKnownPosition = Vector3.zero;

    private bool hasCollisionWithObstacle;
    private bool hasCollisionWithGround;
    [SerializeField]
    private float obstacleCheckRadius = 0.25f;
    private bool isFlipping = false;

    [SerializeField]
    private LayerMask obstacleLayersMask;

    [SerializeField]
    private LayerMask groundLayersMask;

    private WaitForSeconds walkTimeWait;
    private WaitForSeconds idleTimeWait;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        detectorPosition = new Vector3(bc.bounds.extents.x * (isFacingRight ? -1 : 1), bc.bounds.extents.y, 0);
        if(isFacingRight) {
            offset.x *= -1;
        }

        walkTimeWait = new WaitForSeconds(walkTime);
        idleTimeWait = new WaitForSeconds(idleTime);

        detectorPosition += offset;
        idleTime = Mathf.Round(Random.Range(0, 3.5f));

        StartCoroutine(ChangeState());
        StartCoroutine(UpdateLastKnownPosition());
    }

    IEnumerator UpdateLastKnownPosition()
    {
        yield return new WaitForSeconds(3);

        while (enabled)
        {
            if (lastKnownPosition == transform.position && !isFlipping && rb.velocity.y > -0.1f && rb.velocity.y < 0.1f)
            {
                StartCoroutine(Flip());
            }
            lastKnownPosition = transform.position;
            yield return new WaitForSeconds(3);
        }
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
        animator.SetFloat("VelocityX", Mathf.Abs(rb.velocity.x));
    }

    private void FixedUpdate()
    {
        hasCollisionWithObstacle = HasCollision(obstacleLayersMask);
        hasCollisionWithGround = HasCollision(groundLayersMask);
        Vector3 startCast = transform.position - new Vector3(offset.x, 0, 0); ;
        Vector3 endCast = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.9f;
        Debug.DrawLine(startCast, endCast, Color.green);

        RaycastHit2D hitObstacle = Physics2D.Linecast(startCast, endCast, obstacleLayersMask);
        if (!isFlipping && (hitObstacle.collider != null || hasCollisionWithObstacle || !hasCollisionWithGround))
        {
            StartCoroutine(Flip());
        }
    }

    IEnumerator ChangeState()
    {
        while (enabled)
        {
            // Enemy will walk during X seconds...
            isIdle = false;
            yield return walkTimeWait;

            // ...then wait during X seconds...
            isIdle = true;
            yield return idleTimeWait;
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
            rb.velocity = new Vector2(walkSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-walkSpeed, rb.velocity.y);
        }
    }

    private bool HasCollision(LayerMask layerMask)
    {
        return Physics2D.OverlapCircle((Vector2) transform.position - detectorPosition, obstacleCheckRadius, layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2) transform.position - detectorPosition, obstacleCheckRadius);
    }

    private IEnumerator Flip()
    {
        isFlipping = true;
        yield return new WaitForSeconds(0.2f);
        detectorPosition.x *= -1;
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