using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Coroutine autoDestroyCoroutine;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float height;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        height = sr.bounds.size.y;
    }

    public void Initialize()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(
            Random.Range(ScreenUtility.Instance.Left, ScreenUtility.Instance.Right),
            ScreenUtility.Instance.Top + height,
            transform.position.z
        );
    }

    public void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.CompareTag("Player")) {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage();
            Debug.Log("Touché !");
            gameObject.SetActive(false);
        }
    }
}
