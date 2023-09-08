using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    private Image healthBar;

    [SerializeField]
    private GameObject canvas;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        healthBar = canvas.transform.Find("Bar").GetComponent<Image>();
        canvas.SetActive(false);
    }

    private void Start()
    {
        currentLifePoints = enemyData.maxLifePoints;
        UpdateHealth();
    }

    public void TakeDamage(int damage)
    {
        canvas.SetActive(true);
        onHurtBegin?.Invoke();
        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            enemyData.maxLifePoints
        );
        UpdateHealth();

        animator.SetTrigger(AnimationStrings.hurt);
        StartCoroutine(Hurt());

        if (currentLifePoints <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private void UpdateHealth()
    {
        float rate = (float)currentLifePoints / enemyData.maxLifePoints;
        healthBar.fillAmount = rate;
    }

    private IEnumerator Hurt()
    {
        yield return Helpers.GetWait(0.75f);
        onHurtDone?.Invoke();
    }

    private IEnumerator Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        onDeath?.Invoke();
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

        if (enemyData.dropItem != null)
        {
            Instantiate(enemyData.dropItem, transform.position, Quaternion.identity);
        }

        canvas.SetActive(false);
        Destroy(gameObject.transform.parent.gameObject);
    }
}