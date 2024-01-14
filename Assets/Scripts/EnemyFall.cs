using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyFall : MonoBehaviour
{
    private new Collider2D collider;
    private Rigidbody2D rb;
    private EnemyPatrol enemyPatrol;

    [SerializeField]
    private UnityEvent onBegin;

    [SerializeField]
    private EnemyData enemyData;
    private RaycastHit2D hitInfo;

    [SerializeField]
    private LayerMask targetLayers;

    private bool isFalling = false;

    private void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    private void FixedUpdate()
    {
        hitInfo = Physics2D.BoxCast(
            new Vector2(collider.bounds.center.x, collider.bounds.min.y),
            collider.bounds.size,
            0,
            Vector3.down,
            enemyData.distanceDetector,
            targetLayers
        );

        if (hitInfo && !isFalling)
        {
            StartCoroutine(Fall());
        }

        Debug.DrawRay(new Vector2(collider.bounds.max.x, collider.bounds.min.y), Vector3.down * enemyData.distanceDetector, Color.cyan);
        Debug.DrawRay(new Vector2(collider.bounds.min.x, collider.bounds.min.y), Vector3.down * enemyData.distanceDetector, Color.red);
    }

    IEnumerator Fall()
    {
        onBegin?.Invoke();
        isFalling = true;
        yield return null;
        // transform.rotation = Quaternion.Euler(0, 0, 0);
        rb.gravityScale = 15;
        // enemyPatrol.UpdateDetector();
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(isFalling && TryGetComponent(out IDamageable iDamageable)) {
            iDamageable.TakeDamage(int.MaxValue);
        }
    }
}
