using System.Collections;
using UnityEngine;

public class EnemyJumper : MonoBehaviour
{
    [SerializeField]
    private JumperDataValue jumperDataValue;

    private bool isGrounded;

    public float groundCheckRadius = 0.25f;

    public LayerMask listGroundLayers;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;

    private Vector3 offset;

    public EnemyJumperTrigger[] listTriggers;

    private int nextTriggerIndex = 1;

    private bool countUp = true;

    private void Awake()
    {
        enabled = false;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

        offset = new Vector3(0, sr.bounds.extents.y, 0);
    }

    void Start()
    {
        SetTriggersSibling();
        EnableTriggers();

        listTriggers[0].transform.position = transform.position;

        StartCoroutine(JumpAttack(listTriggers[nextTriggerIndex].transform.position));
    }

    void EnableTriggers()
    {
        for (int i = 0; i < listTriggers.Length; i++)
        {
            listTriggers[i].gameObject.SetActive(i == nextTriggerIndex);
        }
    }

    void SetTriggersSibling()
    {
        for (int i = 0; i < listTriggers.Length; i++)
        {
            listTriggers[i].sibling = gameObject;
        }
    }

    public void ChangeTrigger()
    {
        rb.velocity = Vector2.zero;
        nextTriggerIndex = PingPong(nextTriggerIndex);
        EnableTriggers();
        StartCoroutine(JumpAttack(listTriggers[nextTriggerIndex].transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("IsJumping", !isGrounded);
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }

    IEnumerator JumpAttack(Vector2 target)
    {
        yield return new WaitForSeconds(jumperDataValue.delayBetweenJumps);

        Vector2 targetVel = CalculateTrajectoryVelocity(transform.position, target, jumperDataValue.jumpHigh);
        rb.velocity = targetVel;
    }

    private Vector2 CalculateTrajectoryVelocity(Vector3 origin, Vector3 target, float t)
    {
        float vx = (target.x - origin.x) / t;
        float vy = ((target.y - origin.y) - 0.5f * Physics2D.gravity.y * Mathf.Pow(t, 2)) / t;

        return new Vector2(vx, vy);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(transform.position - offset, groundCheckRadius, listGroundLayers);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - offset, groundCheckRadius);
    }

    private int PingPong(int currentValue)
    {
        int nextValue = currentValue;
        if (nextValue <= listTriggers.Length && countUp == true)
        {
            nextValue++;
            if (nextValue == listTriggers.Length)
            {
                nextValue--;
                countUp = false;
            }
        }
        if (nextValue >= 0 && countUp == false)
        {
            nextValue--;
            if (nextValue == 0)
            {
                countUp = true;
            }
        }

        return nextValue;
    }

    private void OnBecameVisible()
    {
        enabled = true;
    }

    private void OnBecameInvisible()
    {
        enabled = false;
    }
}
