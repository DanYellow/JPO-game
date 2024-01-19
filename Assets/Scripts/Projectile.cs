using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour, IRecycleable
{
    private Rigidbody2D rb;
    private new Collider2D collider;

    private bool gotContact = false;

    public ProjectileData projectileData;

    [SerializeField]
    private LayerMask collisionLayers;

    [HideInInspector]
    public IObjectPool<Projectile> pool;

    private bool isStartedTriggered = true;

    [SerializeField]
    private VoidEventChannel resetPlayerPosition;

    [SerializeField]
    private BoolEventChannel onInteractRangeEvent;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        collider = GetComponent<Collider2D>();
        isStartedTriggered = collider.isTrigger;
    }

    private void OnEnable()
    {
        resetPlayerPosition.OnEventRaised += CancelAllProjectiles;
        onInteractRangeEvent.OnEventRaised += ToggleTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(projectileData.damage);
            if (other.gameObject.TryGetComponent(out Knockback knockback))
            {
                knockback.Apply(gameObject, projectileData.knockbackForce);
            }

            IStunnable iStunnable = other.transform.GetComponent<IStunnable>();
            StartCoroutine(iStunnable.Stun(projectileData.stunTime, () => { }));
        }

        Contact();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
            if (iDamageable.GetHealth() > 0)
            {
                iDamageable.TakeDamage(projectileData.damage);
                if (other.gameObject.TryGetComponent(out Knockback knockback))
                {
                    knockback.Apply(gameObject, projectileData.knockbackForce);
                }

                IStunnable iStunnable = other.transform.GetComponent<IStunnable>();
                StartCoroutine(iStunnable.Stun(projectileData.stunTime, () => { }));
            }
        }

        Contact();
    }

    void Contact()
    {
        rb.constraints = 0;
        gotContact = true;
        rb.AddTorque(projectileData.torque, ForceMode2D.Impulse);
        collider.enabled = false;

        StartCoroutine(Disable());
    }

    void CancelAllProjectiles()
    {
        pool.Release(this);
    }

    IEnumerator Disable()
    {
        if (projectileData.blastEffectData)
        {
            BlastEffectData blastEffectData = projectileData.blastEffectData;
            GameObject blast = Instantiate(blastEffectData.effect, new Vector2(collider.bounds.center.x, collider.bounds.min.y), Quaternion.identity);
            // // blast.GetComponent<BlastEffect>().SetEffectData(blastEffectData);
            blast.GetComponent<SpriteRenderer>().color = blastEffectData.color;
            blast.transform.localScale *= blastEffectData.scale;
        }
        yield return Helpers.GetWait(projectileData.blastEffectData ? 0 : 1.5f);
        pool.Release(this);
    }

    void ToggleTime(bool isPaused)
    {
        if (isPaused)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            if (!gotContact)
            {
                rb.constraints = RigidbodyConstraints2D.FreezePositionY;
                rb.velocity = Vector2.zero;
                rb.AddForce(transform.right.normalized * projectileData.speed, ForceMode2D.Impulse);
            }
            else
            {
                rb.constraints = 0;
                rb.AddTorque(projectileData.torque, ForceMode2D.Impulse);
            }
        }
    }

    private void OnDisable()
    {
        resetPlayerPosition.OnEventRaised -= CancelAllProjectiles;
        onInteractRangeEvent.OnEventRaised -= ToggleTime;
    }

    public void ResetThyself()
    {
        gotContact = false;
        collider.enabled = true;
        
        if (Vector3.Dot(transform.up, Vector3.down) > 0) {
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        } else {
            rb.constraints = RigidbodyConstraints2D.FreezePositionY;
        }

        rb.AddForce(transform.right.normalized * projectileData.speed, ForceMode2D.Impulse);
        collider.isTrigger = isStartedTriggered;
    }
}
