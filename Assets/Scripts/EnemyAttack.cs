using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private LayerMask targetLayerMask;

    [SerializeField]
    private EnemyData enemyData;
   
    private BoxCollider2D bc;

    [SerializeField]
    private bool isAttacking = false;
    private Animator animator;
    private EnemyPatrol enemyPatrol;
    //  public ContactFilter2D ContactFilter;

    [SerializeField]
    private float distance = 2f;

    private void Awake()
    {
        // We don't want the script to be enabled by default
        bc = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        enemyPatrol = GetComponent<EnemyPatrol>();
    }

    private void Start()
    {
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitObstacle = Physics2D.BoxCast(
           transform.position,
           new Vector2(1, bc.size.y),
           0,
           enemyPatrol.isFacingRight ? Vector2.right : Vector2.left,
           distance,
           targetLayerMask
       );

        if (hitObstacle && !isAttacking)
        {
            StartCoroutine(Attack());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out IDamageable iDamageable) && other.CompareTag("Player"))
        {
            iDamageable.TakeDamage(2);
        }
    }

    IEnumerator Attack()
    {
        isAttacking = true;
        animator.SetTrigger(AnimationStrings.attack);
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        yield return Helpers.GetWait(enemyData.attackRate);
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(
            new Vector2(enemyPatrol.isFacingRight ? (bc.bounds.max.x + distance) : (bc.bounds.min.x - distance) , transform.position.y),
            // new Vector2((transform.position.x + distance), transform.position.y),
            new Vector2(1, bc.size.y)
        );

        // Gizmos.color = Color.green;
        //  Gizmos.matrix = Matrix4x4.TRS(
        //     transform.position,
        //     transform.rotation,
        //     Vector2.one
        //  );
    }
}
