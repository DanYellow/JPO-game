using UnityEngine;

public class SpriteBeam : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage = 0;
    public float moveSpeed = 1.5f;
    public GameObject impactEffect;

    [HideInInspector]
    public GameObject invoker = null;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * moveSpeed;
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (invoker != other.gameObject)
        {
            if(other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(damage);
            }
            Destroy(gameObject);
            GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(impact, 0.05f);
        }
    }
}
