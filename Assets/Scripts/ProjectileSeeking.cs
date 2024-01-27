using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSeeking : MonoBehaviour
{
    [SerializeField]
    ProjectileData projectileData;

    public Vector3 targetPos;
    private new Collider2D collider;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Awake()
    {
        collider = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        Destroy(gameObject, 5f);
    }

    private void FixedUpdate() {
        rb.MovePosition(rb.transform.position + targetPos * projectileData.speed * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(projectileData.damage);
            if (collision.gameObject.TryGetComponent(out Knockback knockback))
            {
                knockback.Apply(gameObject, projectileData.knockbackForce);
            }

            IStunnable iStunnable = collision.transform.GetComponent<IStunnable>();
            iStunnable.Stun(projectileData.stunTime, () => { });
        }

        Destroy(gameObject);

        if (projectileData.blastEffectData)
        {
            BlastEffectData blastEffectData = projectileData.blastEffectData;
            GameObject blast = Instantiate(blastEffectData.effect, new Vector2(collider.bounds.center.x, collider.bounds.min.y), Quaternion.identity);
            blast.GetComponent<SpriteRenderer>().color = blastEffectData.color;
            blast.transform.localScale *= blastEffectData.scale;
        }
    }
}
