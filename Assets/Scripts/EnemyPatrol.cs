
using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc;

    [SerializeField]
    private EnemyData enemyData;

    [field: SerializeField]
    public bool isFacingRight { get; private set; } = false;

    [HideInInspector]
    public bool isFlipping = false;

    [SerializeField]
    private bool isUpsideDown = false;

    [SerializeField]
    private LayerMask obstacleLayersMask;

    [SerializeField]
    private LayerMask enemyLayersMask;

    private float obstacleDetectionDistance = 1.75f;

    [Header("Raycasts distance")]
    [SerializeField, Tooltip("From what distance the GO can use run speed")]
    private float runDetectionDistance = 2.75f;

    [SerializeField, Tooltip("From what distance the GO can attack")]
    private float attackRange = 0.75f;
    private float voidCheckRadius = 0.2f;

    [SerializeField, Tooltip("From what distance the GO will stop move")]
    private float limitMovementRange = 1.25f;


    [Space(10)]
    [SerializeField, Tooltip("GO cannot attack")]
    private bool enableAttackRange = true;

    [SerializeField, Tooltip("GO cannot run when detect enemy")]
    private bool enableEnemyDetection = true;

    [field: SerializeField, Tooltip("GO stop until distance defined by \"limitMovementRange\"")]
    public bool enableLimitMovementDetection { get; private set; } = false;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Flip();
        }
#endif
    }

    public bool HasTouchedVoid()
    {
        float xOffset = (transform.right.x == -1) ? bc.bounds.min.x : bc.bounds.max.x;
        return !Physics2D.OverlapCircle(
            new Vector2(xOffset, isUpsideDown ? bc.bounds.max.y : bc.bounds.min.y),
            enemyData.obstacleCheckRadius,
            obstacleLayersMask
        );
    }

    public bool HasTouchedObstacle()
    {
        return Physics2D.Linecast(
            new Vector3(transform.position.x + transform.right.x, bc.bounds.min.y + (bc.size.y * 0.10f), 0),
            new Vector3(transform.position.x + (transform.right.x * obstacleDetectionDistance), bc.bounds.min.y + (bc.size.y * 0.10f), 0),
            obstacleLayersMask
        );
    }

    public bool HasReachedLimitZone()
    {
        if (!enableLimitMovementDetection)
        {
            return false;
        }

        float xOffset = (transform.right.x == -1) ? bc.bounds.min.x : bc.bounds.max.x;
        return Physics2D.BoxCast(
            new Vector3(xOffset + bc.size.x * limitMovementRange / 2 * transform.right.x, bc.bounds.center.y, 0),
            new Vector2(bc.size.x * limitMovementRange, bc.size.y),
            rb.rotation,
            transform.right,
            0,
            enemyLayersMask
        );
    }

    public bool HasDetectedEnemy()
    {
        if (!enableEnemyDetection)
        {
            return false;
        }

        float xOffset = (transform.right.x == -1) ? bc.bounds.min.x : bc.bounds.max.x;
        return Physics2D.BoxCast(
            new Vector3(xOffset + bc.size.x * runDetectionDistance / 2 * transform.right.x, bc.bounds.center.y, 0),
            new Vector2(bc.size.x * runDetectionDistance, bc.size.y),
            rb.rotation,
            transform.right,
            0,
            enemyLayersMask
        );
    }

    public RaycastHit2D HasEnemyInAttackRange()
    {
        if (!enableAttackRange)
        {
            RaycastHit2D hit = new RaycastHit2D();
            return hit;
        }

        float xOffset = (transform.right.x == -1) ? bc.bounds.min.x : bc.bounds.max.x;
        return Physics2D.BoxCast(
            new Vector2(xOffset + bc.size.x * attackRange / 2 * transform.right.x, bc.bounds.center.y),
            new Vector2(bc.size.x * attackRange, bc.size.y),
            rb.rotation,
            transform.right,
            0,
            enemyLayersMask
        );
    }
    void OnDrawGizmos()
    {
        if (bc != null)
        {
            float xOffset = (transform.right.x == -1) ? bc.bounds.min.x : bc.bounds.max.x;

            // Detect void area
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(
                new Vector2(xOffset, isUpsideDown ? bc.bounds.max.y : bc.bounds.min.y),
                voidCheckRadius
            );

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

            if (enableEnemyDetection)
            {
                // Detect run area
                Gizmos.color = Color.magenta;
                Gizmos.DrawWireCube(
                    new Vector3(xOffset + bc.size.x * runDetectionDistance / 2 * transform.right.x, bc.bounds.center.y, 0),
                    new Vector2(bc.size.x * runDetectionDistance, bc.size.y)
                );
            }

            if (enableAttackRange)
            {
                // Detect attack area
                Gizmos.color = Color.cyan;
                Gizmos.DrawWireCube(
                    new Vector3(xOffset + bc.size.x * attackRange / 2 * transform.right.x, bc.bounds.center.y, 0),
                    new Vector2(bc.size.x * attackRange, bc.size.y)
                );
            }

            if (enableLimitMovementDetection)
            {
                // Limit movement area
                Gizmos.color = Color.white;
                Gizmos.DrawWireCube(
                    new Vector3(xOffset + bc.size.x * limitMovementRange / 2 * transform.right.x, bc.bounds.center.y, 0),
                    new Vector2(bc.size.x * limitMovementRange, bc.size.y + (bc.size.y * 0.1f))
                );
            }
        }
    }

    public void Flip()
    {
        StartCoroutine(FlipRoutine());
    }

    private IEnumerator FlipRoutine()
    {
        float pauseTime = 1.75f;
        isFlipping = true;
        yield return Helpers.GetWait(pauseTime);
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
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
        enabled = false;
    }

    public EnemyData GetData()
    {
        return enemyData;
    }
}