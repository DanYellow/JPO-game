using UnityEngine.Events;
using UnityEngine;
using System;

public class Shield : MonoBehaviour, IDamageable, IReflectable
{
    [SerializeField]
    private int currentLifePoints;

    private HealthBar healthBar;
    private Enemy boss;

    [SerializeField]
    private GameObject healthBarContainer;

    [SerializeField]
    private EnemyData enemyData;

    private int maxLifePoints;
    private Invulnerable invulnerable;

    [SerializeField]
    private UnityEvent onDestruction;

    private SpriteRenderer sr;

    public bool isReflecting { get; set; } = true;

    public bool hasTotalReflection => true;

    [SerializeField, GradientUsage(true)]
    Gradient colorLevel;

    private void Awake()
    {
        invulnerable = GetComponent<Invulnerable>();
        healthBar = GetComponent<HealthBar>();
        boss = GetComponentInParent<Enemy>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable() {
        if(boss.GetHealth() == 0) {
            return;
        }
        float rate = boss.GetHealthNormalized();
        currentLifePoints = Math.Clamp((int) (enemyData.maxLifePoints * (1 / rate)), enemyData.maxLifePoints, enemyData.maxLifePoints * 2);
        maxLifePoints = currentLifePoints;
        healthBar.maxLifePoints = currentLifePoints;
        healthBar.UpdateContent(currentLifePoints);
        healthBarContainer.SetActive(true);
        sr.material.SetColor("_Color", colorLevel.Evaluate(currentLifePoints / maxLifePoints));
    }

    public int GetHealth() {
        return currentLifePoints;
    }

    public void TakeDamage(int damage)
    {
        if (invulnerable.isInvulnerable) return;

        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            maxLifePoints
        );

        sr.material.SetColor("_Color", colorLevel.Evaluate(currentLifePoints / maxLifePoints));

        healthBar.UpdateContent(currentLifePoints);

        if (currentLifePoints <= 0)
        {
            StopAllCoroutines();
            onDestruction?.Invoke();
            healthBarContainer.SetActive(false);
            gameObject.SetActive(false);

            Animator animator = GetComponentInParent<Animator>();
            animator.SetBool(AnimationStrings.isGuarding, false);
        }
        else
        {
            invulnerable.Trigger();
        }
    }
}
