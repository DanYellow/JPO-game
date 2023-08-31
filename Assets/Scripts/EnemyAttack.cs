using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private LayerMask targetLayerMask;

    private BoxCollider2D bc;

    [SerializeField]
    private bool isFacingRight = false;

    private Vector3 offset;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        bc = GetComponent<BoxCollider2D>();
    }

    private void Start() {
        offset = new((bc.bounds.extents.x * 1.5f) + bc.offset.x, 0, 0);
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitObstacle = Physics2D.BoxCast(
           transform.position + offset,
           new Vector2(1, bc.bounds.max.y),
           0,
           transform.right,
           3,
           targetLayerMask
       );

        if (hitObstacle)
        {
            print(hitObstacle.collider.name);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable iDamageable) && other.CompareTag("Player"))
        {
            iDamageable.TakeDamage(2);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(
            transform.position + offset,
            new Vector2(1, bc.size.y)
        );
    }
}
