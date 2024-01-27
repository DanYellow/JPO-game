using UnityEngine;

public class BreakablePiece : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();    
    }

    void Start()
    {
        rb.AddForce(Random.insideUnitCircle * (Random.Range(0.0f, 1.0f) > 0.5f ? -15 : 15), ForceMode2D.Impulse);
        rb.AddTorque(Random.Range(0.0f, 1.0f) > 0.5f ? -70 : 70);

        Destroy(gameObject, Random.Range(2, 5));
    }
}
