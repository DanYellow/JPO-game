using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IRecycleable
{
    private Rigidbody2D rb;
    private new Collider2D collider;

    public ProjectileData projectileData;

    [SerializeField]
    private LayerMask collisionLayers;

    [HideInInspector]
    public IObjectPool<Projectile> pool;

    private bool isStartedTriggered = true;

    [SerializeField]
    private VoidEventChannel resetPlayerPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        isStartedTriggered = collider.isTrigger;
    }

    private void OnEnable()
    {
        resetPlayerPosition.OnEventRaised += CancelAllProjectiles;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Contact();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Contact();
            IDamageable iDamageable = collision.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(projectileData.damage);
            if (collision.gameObject.TryGetComponent(out Knockback knockback))
            {
                knockback.Apply(gameObject, projectileData.knockbackForce);
            }
        }
    }

    void Contact()
    {
        rb.constraints = 0;
        rb.AddTorque(projectileData.torque, ForceMode2D.Impulse);
        collider.isTrigger = true;

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

    private void OnDisable()
    {
        // collider.isTrigger = false;
        resetPlayerPosition.OnEventRaised -= CancelAllProjectiles;
    }

    public void ResetThyself()
    {
        rb.AddForce(transform.right.normalized * projectileData.speed, ForceMode2D.Impulse);
        collider.isTrigger = isStartedTriggered;
        // rb.velocity = transform.right.normalized * projectileData.speed;
        // print(rb.velocity);

        // rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezePositionY;
    }
}
