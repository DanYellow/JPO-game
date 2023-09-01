using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc2d;

    [SerializeField]
    private ProjectileData projectileData;

    [SerializeField]
    private LayerMask collisionLayers;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        bc2d = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        rb.AddForce(Vector2.left * projectileData.speed, ForceMode2D.Impulse);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        rb.constraints = 0;
        rb.AddTorque(projectileData.torque, ForceMode2D.Impulse);
        bc2d.isTrigger = true;

        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(projectileData.damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (bc2d == null)
        {
            bc2d = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        Gizmos.DrawLine(new Vector2(bc2d.bounds.min.x, bc2d.bounds.center.y), new Vector2(bc2d.bounds.min.x + (10 * Mathf.Sign(rb.velocity.x)), bc2d.bounds.center.y));
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
