
using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    public float speed;

    private SpriteRenderer sr;

    public bool isFacingRight = false;

    private bool isIdle = true;

    private float idleTime;

    [SerializeField]
    private Vector3 offset;
    public Vector3 rightOffset = new Vector3(-0.2f, 0.25f, 0);

    [SerializeField]
    private Vector3 lastKnownPosition = Vector3.zero;

    private bool isGrounded;
    private bool isFlipping = false;

    [Tooltip("Define how long the enemy will walk")]
    public float walkTime = 5f;

    public LayerMask layerMask;
    public float groundCheckRadius = 0.25f;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        enabled = false;
        sr = GetComponent<SpriteRenderer>();

        offset = new Vector3(sr.bounds.size.x / 4, sr.bounds.size.y / 2, 0);
    }

    private void Start()
    {
        idleTime = Mathf.Round(Random.Range(0, 3.5f));
        StartCoroutine(ChangeState());

        InvokeRepeating(nameof(UpdateLastKnownPosition), 3.0f, 3f);
    }

    void UpdateLastKnownPosition() {
        if(lastKnownPosition == transform.position && !isFlipping) {
            StartCoroutine(Flip());
        }
        lastKnownPosition = transform.position;
    }

    private void Update()
    {
        if (isIdle)
        {
            Idle();
        }
        else
        {
            Move();
        }

        animator.SetFloat("MoveDirectionX", Mathf.Abs(rb.velocity.x));
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        Vector3 startCast = transform.position;
        Vector3 endCast = transform.position + (isFacingRight ? Vector3.right : Vector3.left) * 0.5f;
        Debug.DrawLine(startCast, endCast, Color.green);

        RaycastHit2D hit = Physics2D.Linecast(transform.position, endCast, layerMask);

        if ((hit.collider != null && hit.distance < 0.4f) || !isGrounded)
        {
            StartCoroutine(Flip());
        }
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position - offset, groundCheckRadius, layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - offset, groundCheckRadius);
    }

    IEnumerator ChangeState()
    {
        while (true)
        {
            // Enemy will walk during X seconds...
            isIdle = false;
            yield return new WaitForSeconds(walkTime);

            // ...then wait during X seconds...
            isIdle = true;
            yield return new WaitForSeconds(idleTime);
        }
    }

    private void Idle()
    {
        rb.velocity = Vector2.zero;
    }

    private void Move()
    {
        if (isFacingRight)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
        }
    }

    IEnumerator Flip()
    {
        offset.x *= -1;
        isFlipping = true;
        isFacingRight = !isFacingRight;
        transform.Rotate(0f, 180f, 0f);
        lastKnownPosition = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        isFlipping = false;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        // We stop the enemy when is not visible or else
        // it might continue to run but whoen be able to change direction
        Idle();
        enabled = false;
    }
}