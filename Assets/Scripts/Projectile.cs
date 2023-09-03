using System.Collections;
using UnityEngine;
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

    [SerializeField]
    private VoidEventChannel resetPlayerPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
    }

     private void OnEnable()
    {
        resetPlayerPosition.OnEventRaised += CancelAllProjectiles;
    }

    // private void OnCollisionEnter2D(Collision2D other)
    // {
    //     rb.constraints = 0;
    //     rb.AddTorque(projectileData.torque, ForceMode2D.Impulse);
    //     bc2d.isTrigger = true;

    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
    //         iDamageable.TakeDamage(projectileData.damage);
    //         if(other.gameObject.TryGetComponent(out Knockback knockback)) {
    //             knockback.Apply(gameObject, projectileData.knockbackForce);
    //         }
    //     }
    //     StartCoroutine(Disable());
    // }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        rb.constraints = 0;
        rb.AddTorque(projectileData.torque, ForceMode2D.Impulse);
        bc2d.isTrigger = true;

        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(projectileData.damage);
            if(collision.gameObject.TryGetComponent(out Knockback knockback)) {
                knockback.Apply(gameObject, projectileData.knockbackForce);
            }
        }
        StartCoroutine(Disable());
    }

    void CancelAllProjectiles()
    {
        pool.Release(this);
    }

    IEnumerator Disable()
    {
        yield return Helpers.GetWait(1.5f);
        pool.Release(this);
    }

    private void OnDisable() {
        // bc2d.isTrigger = false;
        resetPlayerPosition.OnEventRaised -= CancelAllProjectiles;
    }

    public void ResetThyself()
    {
        rb.AddForce(transform.right.normalized * projectileData.speed, ForceMode2D.Impulse);
        // rb.velocity = transform.right.normalized * projectileData.speed;
        // print(rb.velocity);
        
        // rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }
}
