using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetLayerMask;

    [SerializeField]
    private EnemyData enemyData;

    private BoxCollider2D bc2d;

    [SerializeField]
    private bool isAttacking = false;
    private Animator animator;
    private EnemyPatrol enemyPatrol;
    //  public ContactFilter2D ContactFilter;
    public UnityEvent OnBegin, OnDone;

    [SerializeField]
    private float distance = 2f;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        bc2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitObstacle = Physics2D.BoxCast(
           new Vector2(bc2d.bounds.center.x - 0.5f, bc2d.bounds.center.y),
           bc2d.bounds.size,
           0,
           enemyPatrol.isFacingRight ? Vector2.right : Vector2.left,
           distance,
           targetLayerMask
       );

        Debug.DrawRay(new Vector2(enemyPatrol.isFacingRight ? bc2d.bounds.max.x : bc2d.bounds.min.x, bc2d.bounds.min.y), (enemyPatrol.isFacingRight ? Vector2.right : Vector2.left) * distance, Color.cyan);
        Debug.DrawRay(new Vector2(enemyPatrol.isFacingRight ? bc2d.bounds.max.x : bc2d.bounds.min.x, bc2d.bounds.max.y), (enemyPatrol.isFacingRight ? Vector2.right : Vector2.left) * distance, Color.cyan);
        // Debug.DrawRay(new Vector2(bc2d.bounds.min.x - 0.25f, bc2d.bounds.max.y), enemyPatrol.isFacingRight ? Vector2.right : Vector2.left * distance, Color.cyan);

        if (hitObstacle && !isAttacking)
        {
            // StartCoroutine(Attack());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && isAttacking)
        {            
            if(other.gameObject.TryGetComponent(out Knockback knockback)) {
                knockback.Apply(gameObject, enemyData.knockbackForce);
            }

            if(other.gameObject.TryGetComponent(out IDamageable iDamageable)) {
               iDamageable.TakeDamage(enemyData.damage);
            }
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        OnBegin?.Invoke();
        animator.SetTrigger(AnimationStrings.attack);
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        OnDone?.Invoke();
        yield return Helpers.GetWait(enemyData.attackRate);
        isAttacking = false;
    }
}
