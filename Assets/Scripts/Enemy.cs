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

    private Image healthBar;

    [SerializeField]
    private GameObject canvas;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        // canvas = transform.parent.GetComponentInChildren<Canvas>();
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
        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            enemyData.maxLifePoints
        );
        UpdateHealth();

        animator.SetTrigger(AnimationStrings.hurt);

        if (currentLifePoints <= 0)
        {
            StartCoroutine(Die());
        }
        else { }
    }

    private void UpdateHealth()
    {
        float rate = (float)currentLifePoints / enemyData.maxLifePoints;
        healthBar.fillAmount = rate;
    }

    private IEnumerator Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        onDeath?.Invoke();
        if (animator)
        {
            animator.SetBool(AnimationStrings.isDead, true);
            yield return null;
            // yield return new WaitUntil(() => animator.GetCurrentAnimatorClipInfo(0)[0].clip.name == $"{enemyData.name}Die");
            yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
            yield return Helpers.GetWait(0.25f);
        }
        else
        {
            yield return null;
        }
        canvas.SetActive(false);
        Destroy(gameObject.transform.parent.gameObject);
    }
}