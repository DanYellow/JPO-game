using UnityEngine;

public class SpriteBeam : MonoBehaviour
{
    private Rigidbody2D rb;
    public float damage = 0;
    public GameObject invoker = null;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * 1.5f;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (
            other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable) &&
            invoker != other.gameObject
        )
        {
            iDamageable.TakeDamage(damage);
        }

        if (invoker != other.gameObject)
        {
            Destroy(gameObject);
        }

    }
}
