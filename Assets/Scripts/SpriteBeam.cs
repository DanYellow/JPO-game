using UnityEngine;

public class SpriteBeam : MonoBehaviour
{
    private Rigidbody2D rb;
    public GameObject impactEffect;

    [SerializeField]
    private BeamValue beamData;

    [HideInInspector]
    public float damageFactor = 1;
    [HideInInspector]
    public float moveSpeedFactor = 1;

    [HideInInspector]
    public GameObject invoker = null;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        rb.velocity = transform.right * beamData.moveSpeed * moveSpeedFactor;
        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (invoker != other.gameObject)
        {
            if(other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable)) {
                iDamageable.TakeDamage(beamData.damage * damageFactor);
            }
            Destroy(gameObject);
            GameObject impact = Instantiate(impactEffect, transform.position, Quaternion.identity);
            Destroy(impact, 0.05f);
        }
    }
}
