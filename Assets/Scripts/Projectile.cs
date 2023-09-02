using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IRecycleable
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

    IEnumerator Disable()
    {
        yield return Helpers.GetWait(1.5f);
        pool.Release(this);
    }

    private void OnDisable() {
        bc2d.isTrigger = false;
        rb.velocity = Vector2.zero;
    }

    public void ResetThyself()
    {
        rb.AddForce(transform.right.normalized * projectileData.speed, ForceMode2D.Impulse);
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }
}
