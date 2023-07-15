using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float height;
    private int nbInvocations = 0;
    public float maxLinearDrag = 4.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        height = sr.bounds.size.y;
        rb.drag = maxLinearDrag;
    }

    private void FixedUpdate()
    {
        // rb.velocity += 0.5f * Time.fixedDeltaTime;
    }

    public void Initialize()
    {
        if(nbInvocations % 5 == 0) {
            rb.drag = Mathf.Clamp(rb.drag - 0.5f, 0, maxLinearDrag);
        }
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(
            Random.Range(ScreenUtility.Instance.Left, ScreenUtility.Instance.Right),
            ScreenUtility.Instance.Top + height,
            transform.position.z
        );
    }

    public void OnBecameInvisible()
    {
        nbInvocations++;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage();
            Debug.Log("Touché !");
            gameObject.SetActive(false);
        }
    }
}
