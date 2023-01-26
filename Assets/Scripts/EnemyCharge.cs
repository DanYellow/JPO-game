using System.Collections;
using UnityEngine;

public class EnemyCharge : MonoBehaviour
{
    private Vector3 offset;
    private SpriteRenderer sr;
    private Rigidbody2D rb;

    public EnemyStatsValue enemyData;

    private bool isWallColliding;

    [SerializeField]
    private bool isFacingRight = false;
    private bool isFlipping = false;
    private bool isAttacking = false;

    public LayerMask obstacleLayersMask;
    public LayerMask wallLayersMask;

    [Header("Components to disable after specific event. E.g. : death")]
    public Behaviour[] listDisabledBehaviours;


    private void Awake()
    {
        enabled = false;
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        offset = new Vector3(sr.bounds.extents.x * (isFacingRight ? -1 : 1), 0, 0);
    }

    private void Update()
    {
        foreach (Behaviour component in listDisabledBehaviours)
        {
            component.enabled = !isAttacking;
        }
    }

    private void FixedUpdate()
    {
        isWallColliding = IsWallColliding();
        Vector3 startCast = transform.position - new Vector3(offset.x, 0, 0);
        Vector3 endCast = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * enemyData.sightLength;

        Debug.DrawLine(startCast, endCast, Color.white);

        RaycastHit2D hitObstacle = Physics2D.Linecast(startCast, endCast, obstacleLayersMask);

        if (hitObstacle.collider != null && hitObstacle.collider.gameObject.CompareTag("Player") && !isAttacking)
        {
            isAttacking = true;
            rb.velocity += -hitObstacle.normal * (1 * enemyData.moveSpeed);
        }

        if (isWallColliding && !isFlipping)
        {
            StartCoroutine(Flip());
        }
    }

    public bool IsWallColliding()
    {
        return Physics2D.OverlapCircle(transform.position - offset, enemyData.wallCheckRadius, wallLayersMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position - offset, enemyData.wallCheckRadius);
    }

    IEnumerator Flip()
    {
        isFlipping = true;
        yield return new WaitForSeconds(1.5f);
        offset.x *= -1;
        isAttacking = false;
        isFlipping = false;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }
}
