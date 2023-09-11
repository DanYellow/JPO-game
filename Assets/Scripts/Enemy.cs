using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField]
    private EnemyData enemyData;

    [SerializeField]
    private int currentLifePoints;

    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField]
    private UnityEvent onDeath;

    [SerializeField]
    private UnityEvent onHurtBegin;

    [SerializeField]
    private UnityEvent onHurtDone;


    [HideInInspector]
    public Action<GameObject> deathNotify;

    private IsGrounded isGrounded;

    private Invulnerable invulnerable;

    private bool isDying = false;

    private HealthBar healthBar;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        isGrounded = GetComponent<IsGrounded>();
        invulnerable = GetComponent<Invulnerable>();
        healthBar = GetComponent<HealthBar>();
    }

    private void Start()
    {
        currentLifePoints = enemyData.maxLifePoints;

        healthBar.UpdateContent(currentLifePoints);
    }

    private void Update()
    {
        animator.SetFloat(AnimationStrings.velocityY, rb.velocity.y);
        if (isGrounded != null)
        {
            animator.SetBool(AnimationStrings.isGrounded, isGrounded.isGrounded);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDying) return;

        onHurtBegin?.Invoke();
        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            enemyData.maxLifePoints
        );

        healthBar.UpdateContent(currentLifePoints);

        animator.SetTrigger(AnimationStrings.hurt);
        animator.SetBool(AnimationStrings.canMove, false);
        StartCoroutine(Hurt());

        if (currentLifePoints <= 0)
        {
            isDying = true;
            StartCoroutine(Die());
        }
        else
        {
            if (invulnerable != null)
            {
                invulnerable.Trigger();
            }
        }
    }

    private IEnumerator Hurt()
    {
        yield return null;
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        // yield return Helpers.GetWait(0.75f);
        animator.SetBool(AnimationStrings.canMove, true);
        onHurtDone?.Invoke();
    }

    private IEnumerator Die()
    {
        onDeath?.Invoke();
        deathNotify?.Invoke(gameObject.transform.root.gameObject);
        rb.velocity = Vector2.zero;
        if (animator)
        {
            animator.SetBool(AnimationStrings.isDead, true);

            yield return null;
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
            yield return Helpers.GetWait(0.3f);
        }
        else
        {
            yield return null;
        }

        float drop = UnityEngine.Random.Range(0f, 1f);

        if (enemyData.dropItem != null && drop < enemyData.dropProbability)
        {
            Instantiate(enemyData.dropItem, transform.position, Quaternion.identity);
        }

        if (enemyData.blastEffect != null)
        {
            Instantiate(enemyData.blastEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject.transform.root.gameObject);
    }

    private void OnDestroy()
    {

    }
}