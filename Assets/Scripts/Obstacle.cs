using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private Animator animator;

    private float height;
    private int nbInvocations = 0;
    private int accelerationFallThreshold = 2;
    public float maxLinearDrag = 3.5f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        height = sr.bounds.size.y;
        rb.drag = maxLinearDrag;
    }

    private void OnEnable()
    {
        if (nbInvocations % accelerationFallThreshold == 0)
        {
            rb.drag = Mathf.Clamp(rb.drag - 0.75f, 0, maxLinearDrag);
        }
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        animator.ResetTrigger("Touched");

        transform.position = new Vector3(
            Random.Range(
                ScreenUtility.Instance.Left + (sr.bounds.size.x / 2),
                ScreenUtility.Instance.Right - (sr.bounds.size.x / 2)
            ),
            ScreenUtility.Instance.Top + height,
            transform.position.z
        );
    }

    private void Update()
    {
        if (transform.position.y < ScreenUtility.Instance.Bottom - height)
        {
            nbInvocations++;
            gameObject.SetActive(false);
        }
    }

    IEnumerator OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(0);

            rb.constraints = RigidbodyConstraints2D.FreezeAll;
            animator.SetTrigger("Touched");

            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            gameObject.SetActive(false);
        }
    }
}

// yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).IsName("ObstacleExplosion"));

// Wait for the transition to end
// yield return new WaitUntil(() => _Animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1.0f );

// Wait for the animation to end
// yield return new WaitWhile(() => _Animator.GetCurrentAnimatorStateInfo(0).IsName("anim_state"));