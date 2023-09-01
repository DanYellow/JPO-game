using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D bc2d;

    public ProjectileData projectileData;

    [SerializeField]
    private LayerMask collisionLayers;

    [HideInInspector]
    public IObjectPool<Projectile> pool;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        bc2d = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // Initialize();
        // rb.AddForce(Vector2.left * projectileData.speed, ForceMode2D.Impulse);
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
        StartCoroutine(Disable());
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

    IEnumerator Disable()
    {
        yield return Helpers.GetWait(1.5f);
        pool.Release(this);
    }


    private void OnEnable() {
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        rb.AddForce(Vector2.left * projectileData.speed, ForceMode2D.Impulse);
    }

    // private void OnBecameInvisible()
    // {
    //     print("ffff");
    //     pool.Release(this);
    //     // gameObject.SetActive(false);
    // }

    private void OnDisable() {
        bc2d.isTrigger = false;
        rb.velocity = Vector2.zero;
    }
}
