using System.Collections;
using UnityEngine;

public class EnemyFall : MonoBehaviour
{
    private BoxCollider2D bc;
    private Rigidbody2D rb;

    private Animator animator;

    [SerializeField]
    private EnemyData enemyData;
    private RaycastHit2D hitInfo;

    [SerializeField]
    private LayerMask targetLayers;

    private bool isFalling = false;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        hitInfo = Physics2D.BoxCast(
            new Vector2(bc.bounds.center.x, bc.bounds.min.y),
            new Vector2(bc.bounds.size.x * 1.1f, bc.bounds.size.y),
            0,
            Vector3.down,
            enemyData.distanceDetector,
            targetLayers
        );

        if (hitInfo && !isFalling)
        {
            StartCoroutine(Fall());
        }
    }

    void OnDrawGizmos()
    {
        if (bc != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(
                new Vector2(bc.bounds.min.x - (bc.size.y * 0.05f), bc.bounds.min.y),
                new Vector2(bc.bounds.min.x - (bc.size.y * 0.05f), bc.bounds.min.y - enemyData.distanceDetector)            
            );
            Gizmos.DrawLine(
                new Vector2(bc.bounds.max.x + (bc.size.y * 0.05f), bc.bounds.min.y),
                new Vector2(bc.bounds.max.x + (bc.size.y * 0.05f), bc.bounds.min.y - enemyData.distanceDetector)            
            );
        }
    }

    IEnumerator Fall()
    {
        animator.SetTrigger(AnimationStrings.fall);
        isFalling = true;
        yield return null;
        rb.gravityScale = 15;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (isFalling && TryGetComponent(out IDamageable iDamageable))
        {
            bc.enabled = false;
            iDamageable.TakeDamage(int.MaxValue);
        }
    }
}
