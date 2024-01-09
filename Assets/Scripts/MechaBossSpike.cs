using UnityEngine;

// https://www.youtube.com/watch?v=IGDrF1Cq9Q0

public class MechaBossSpike : MonoBehaviour
{
    [SerializeField]
    ProjectileData projectileData;

    private bool throwing = false;
    private bool isDestroyed = false;

    private SpriteRenderer sr;

    public Vector3 throwDir;

    public Quaternion origRotation { private set; get; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        origRotation = transform.rotation;
    }

    private void Update()
    {
        if (throwing && !isDestroyed)
        {
            transform.position += projectileData.speed * Time.deltaTime * throwDir;
        }

        if(isDestroyed) {
            transform.position += projectileData.speed / 2 * Time.deltaTime * Vector3.down;
        }
    }

    public void Destroy() {
        isDestroyed = true;
    }
    
    public void Throw(Vector3 _throwDir)
    {
        throwDir = _throwDir;
        throwing = true;

        sr.color = Color.red;
    }

    public void Reset()
    {
        throwing = false;
        sr.color = Color.white;
        transform.rotation = origRotation;
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
        }
    }
}
