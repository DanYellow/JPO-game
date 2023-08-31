using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyStats enemyStats;

    [SerializeField]
    private int currentLifePoints;

    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField]
    private UnityEvent onDeath;

    private void Awake() {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        currentLifePoints = enemyStats.maxLifePoints;
    }

    public void TakeDamage(int damage)
    {
        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            enemyStats.maxLifePoints
        );

        animator.SetTrigger(AnimationStrings.hurt);

        if (currentLifePoints <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        onDeath?.Invoke();

        if(animator) {
            animator.SetBool(AnimationStrings.isDead, true);
            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 1.5f);
            Destroy(gameObject);
        } else {
            yield return null;
            Destroy(gameObject);
        }
    }
}