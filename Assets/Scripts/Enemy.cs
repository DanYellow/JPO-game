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
        } else {}
    }

    private IEnumerator Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        onDeath?.Invoke();

        if(animator) {
            // yield return null;
            // print("hurt " + animator.GetCurrentAnimatorStateInfo(0).length);
            animator.SetBool(AnimationStrings.isDead, true);

            // yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            print(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
            yield return Helpers.GetWait(0.25f);


            // print("hurt " + animator.GetCurrentAnimatorStateInfo(0).length);
            Destroy(gameObject);
        } else {
            yield return null;
            Destroy(gameObject);
        }
    }
}