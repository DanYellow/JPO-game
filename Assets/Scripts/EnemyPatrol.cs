
using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D bc;

    [SerializeField]
    private EnemyData enemyData;

    [field: SerializeField]
    public bool isFacingRight { get; private set; } = false;

    [SerializeField]
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
    private bool canMove = true;

    public bool isFlipping = false;

    [SerializeField]
    private LayerMask obstacleLayersMask;

    [SerializeField]
    private LayerMask groundLayersMask;

    [SerializeField]
    private LayerMask enemyLayersMask;

    public Vector2 lastPosition = Vector2.zero;

    private float obstacleDetectionDistance = 1.95f;
    private float runDetectionDistance = 2.75f; //3.55f;
    private float attackRange = 0.75f;

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
        if (isFacingRight)
        {
            offset.x *= -1;
        }

        lastPosition = rb.position;

        detectorPosition += offset;
        idleTime = Mathf.Round(Random.Range(0, 3.5f));

        // StartCoroutine(ChangeState());
        // StartCoroutine(UpdateLastKnownPosition());
    }

    public void UpdateDetector()
    {
        detectorPosition = new Vector3(bc.bounds.extents.x * (isFacingRight ? -1 : 1), bc.bounds.extents.y, 0);
        if (isFacingRight)
        {
            offset.x *= -1;
        }
        detectorPosition += offset;
    }

    IEnumerator UpdateLastKnownPosition()
    {
        yield return Helpers.GetWait(2);

        while (enabled)
        {
            if (canMove && lastKnownPosition == transform.position && !isFlipping && rb.velocity.y > -0.1f && rb.velocity.y < 0.1f)
            {
                StartCoroutine(Flip());
            }
            lastKnownPosition = transform.position;
            yield return Helpers.GetWait(2f);
        }
    }

    private void Update()
    {
        // if (isIdle)
        // {
        //     Idle();
        // }
        // animator.SetFloat(AnimationStrings.velocityX, Mathf.Abs(rb.velocity.x));

        if(Input.GetKeyDown(KeyCode.Space)) {
            Flipp();
        }
    }

    private void FixedUpdate()
    {
        // hasCollisionWithObstacle = HasCollision(obstacleLayersMask);
        // hasCollisionWithGround = HasCollision(groundLayersMask);
        // Vector3 startCast = transform.position - new Vector3(offset.x, 0, 0);
        // Vector3 endCast = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.9f;
        // Debug.DrawLine(startCast, endCast, Color.green);

        // RaycastHit2D hitObstacle = Physics2D.Linecast(startCast, endCast, obstacleLayersMask);
        // hitObstacle.collider != null || 
        // if (!isFlipping && (hasCollisionWithObstacle || !hasCollisionWithGround))
        // {
        //     StartCoroutine(Flip());
        // }

        // if (!isIdle)
        // {
        //     Move();
        // }
    }

    IEnumerator ChangeState()
    {
        while (enabled)
        {
            // Enemy will walk during X seconds...
            isIdle = false;
            yield return Helpers.GetWait(walkTime);

            // ...then wait during X seconds...
            isIdle = true;
            yield return Helpers.GetWait(idleTime);
        }
    }

    private void Idle()
    {
        rb.velocity = Vector2.zero;
    }

    private void Move()
    {
        if (canMove)
        {
            rb.velocity = new Vector2(
                enemyData.walkSpeed * (isFacingRight ? 1 : -1),
                rb.velocity.y
            );
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    public bool HasTouchedGround()
    {
        return HasCollision(obstacleLayersMask);
    }

    public bool HasTouchedObstacle()
    {
        return Physics2D.Linecast(
            new Vector3(transform.position.x + transform.right.x, bc.bounds.min.y + (bc.size.y * 0.10f), 0),
            new Vector3(transform.position.x + (transform.right.x * obstacleDetectionDistance), bc.bounds.min.y + (bc.size.y * 0.10f), 0),
            obstacleLayersMask
        );
    }

    public bool HasDetectedEnemy()
    {
        return Physics2D.BoxCast(
            new Vector3(bc.bounds.max.x + (bc.size.x * runDetectionDistance / 2), bc.bounds.center.y, 0),
            new Vector2(bc.size.x * runDetectionDistance, bc.size.y),
            rb.rotation,
            transform.right,
            0,
            enemyLayersMask
        );
    }

    public RaycastHit2D HasEnemyInAttackRange()
    {
        float xOffset = transform.right.x == -1 ? bc.bounds.min.x : bc.bounds.max.x;
        return Physics2D.BoxCast(
            new Vector2(xOffset + bc.size.x * attackRange / 2 * transform.right.x, bc.bounds.center.y),
            new Vector2(bc.size.x * attackRange, bc.size.y),
            rb.rotation,
            transform.right,
            0,
            enemyLayersMask
        );
    }

    public bool HasCollision(LayerMask layerMask)
    {
        return Physics2D.OverlapCircle((Vector2)transform.position - detectorPosition, enemyData.obstacleCheckRadius, layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position - detectorPosition, enemyData.obstacleCheckRadius);
        // print(transform.right);

        if (bc != null)
        {
            // Detect top obstacle
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector3(transform.position.x + transform.right.x, bc.bounds.min.y + (bc.size.y * 0.10f), 0),
                new Vector3(transform.position.x + (transform.right.x * obstacleDetectionDistance), bc.bounds.min.y + (bc.size.y * 0.10f), 0)
            );

            // Detect bottom obstacle
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                new Vector3(transform.position.x + transform.right.x, bc.bounds.max.y - (bc.size.y * 0.10f), 0),
                new Vector3(transform.position.x + (transform.right.x * obstacleDetectionDistance), bc.bounds.max.y - (bc.size.y * 0.10f), 0)
            );

            float xOffset = transform.right.x == -1 ? bc.bounds.min.x : bc.bounds.max.x;
            // Detect run area
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireCube(
                new Vector3(xOffset + bc.size.x * runDetectionDistance / 2 * transform.right.x, bc.bounds.center.y, 0),
                new Vector2(bc.size.x * runDetectionDistance, bc.size.y)
            );

            // Detect attack area
            Gizmos.color = Color.cyan;
            float factor = 3;
            Gizmos.DrawWireCube(
                new Vector3(xOffset + bc.size.x * attackRange / 2 * transform.right.x, bc.bounds.center.y, 0),
                new Vector2(bc.size.x * attackRange, bc.size.y)
            );
        }
    }

    public void ToggleMovement(bool val)
    {
        canMove = val;
    }

    public void Flipp()
    {
        StartCoroutine(Flip());
    }

    private IEnumerator Flip()
    {
        float pauseTime = 1.75f;
        isFlipping = true;
        yield return Helpers.GetWait(pauseTime);
        detectorPosition.x *= -1;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        lastKnownPosition = Vector3.zero;
        yield return Helpers.GetWait(pauseTime);
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

    public EnemyData GetData()
    {
        return enemyData;
    }
}