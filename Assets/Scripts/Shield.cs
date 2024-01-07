using UnityEngine.Events;
using UnityEngine;

public class Shield : MonoBehaviour, IDamageable, IReflectable
{
    [ReadOnlyInspector]
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

    public bool isReflecting { get; set; } = true;

    public bool hasTotalReflection => true;

    private void Awake()
    {
        invulnerable = GetComponent<Invulnerable>();
        healthBar = GetComponent<HealthBar>();
        boss = GetComponentInParent<Enemy>();
    }

    private void OnEnable() {
        float rate = (float)boss.GetHealth() / boss.GetMaxHealth();
        currentLifePoints = enemyData.maxLifePoints * (int) Mathf.Round(1.25f / rate);
        maxLifePoints = currentLifePoints;
        healthBar.maxLifePoints = currentLifePoints;
        healthBar.UpdateContent(currentLifePoints);
        healthBarContainer.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (invulnerable.isInvulnerable) return;

        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            maxLifePoints
        );

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
