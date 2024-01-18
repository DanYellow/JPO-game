using System.Collections;
using UnityEngine;
using UnityEngine.Events;

// https://www.youtube.com/watch?v=hrvWkMimY1k

public class Guard : MonoBehaviour, IGuardable
{
    private Animator animator;
    private BoxCollider2D bc;
    private Rigidbody2D rb;


    [SerializeField]
    private LayerMask targetLayerMask;

    public bool isGuarding { get; set; } = false;
    public bool hasTotalGuard { get; } = false;

    private RaycastHit2D hitObstacle;

    private Enemy enemy;

    private float shieldRange = 0.1f;

    public float guardCountdown = 0;
    public float guardDuration = 2.5f;
    public bool startCountdown = false;

    public bool canGuard = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        bc = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
    }

    private void Update() {
        if(guardCountdown > 0) {
            guardCountdown -= Time.deltaTime;
        }
    }

    public bool CanGuard() {
        return guardCountdown <= 0;
    }

    public void ResetTimer() {
        
        StartCoroutine(ResetMovements());
    }

    IEnumerator ResetMovements() {
        yield return Helpers.GetWait(guardDuration / 2);
        guardCountdown = guardDuration;
        // enemy.canMove = true;
    }

    // public

    private void FixedUpdate()
    {
        float xOffset = transform.right.x == -1 ? bc.bounds.min.x : bc.bounds.max.x;

        hitObstacle = Physics2D.BoxCast(
           new Vector2(xOffset + bc.size.x * shieldRange / 2 * transform.right.x, bc.bounds.center.y),
           new Vector2(bc.size.x * shieldRange, bc.size.y),
           rb.rotation,
           transform.right,
           0,
           targetLayerMask
       );

        if (hitObstacle.collider != null)
        {
            Reflect(hitObstacle.collider.transform);
        }

        //     Debug.DrawRay(new Vector2(enemyPatrol.isFacingRight ? bc.bounds.max.x : bc.bounds.min.x, bc.bounds.min.y), (enemyPatrol.isFacingRight ? Vector2.right : Vector2.left) * distance, Color.cyan);
        //     Debug.DrawRay(new Vector2(enemyPatrol.isFacingRight ? bc.bounds.max.x : bc.bounds.min.x, bc.bounds.max.y), (enemyPatrol.isFacingRight ? Vector2.right : Vector2.left) * distance, Color.cyan);

        //     if (hitObstacle)
        //     {
        //         if (!isGuarding)
        //         {
        //             StartCoroutine(Protect());
        //         }
        //         else
        //         {
        //             Reflect();
        //         }
        //     }
    }

    public void ProtectProxy() {
        StopAllCoroutines();
        StartCoroutine(Protect());
    }

    IEnumerator Protect()
    {
        isGuarding = true;
        // OnBegin?.Invoke();
        // animator.SetBool(AnimationStrings.isGuarding, isGuarding);
        yield return null;
        yield return Helpers.GetWait(2.15f);
        print("guard");
        // OnDone?.Invoke();
        isGuarding = false;
        animator.SetBool(AnimationStrings.isGuarding, isGuarding);
    }

    void OnDrawGizmos()
    {
        if (bc != null)
        {
            Gizmos.color = Color.yellow;
            float xOffset = transform.right.x == -1 ? bc.bounds.min.x : bc.bounds.max.x;
            Gizmos.DrawWireCube(
                new Vector3(xOffset + bc.size.x * shieldRange / 2 * transform.right.x, bc.bounds.center.y, 0),
                new Vector2(bc.size.x * shieldRange, bc.size.y)
            );
        }
    }

    private void Reflect(Transform obstacle)
    {
        if (obstacle.right.x != transform.right.x && obstacle.TryGetComponent(out Knockback knockback))
        {
            knockback.Apply(gameObject, 5);
        }
        // IAttackable iAttackable = hitObstacle.transform.GetComponentInChildren<IAttackable>();
        // if (iAttackable != null && iAttackable.isAttacking)
        // {
        //     if (hitObstacle.transform.TryGetComponent(out Knockback knockback))
        //     {
        //         knockback.Apply(gameObject, 250);
        //     }
        // }
    }

}
