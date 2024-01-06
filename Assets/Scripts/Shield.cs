using UnityEngine.Events;
using UnityEngine;

public class Shield : MonoBehaviour, IDamageable, IReflectable
{
    [ReadOnlyInspector]
    private int currentLifePoints;

    private HealthBar healthBar;

    [SerializeField]
    private GameObject healthBarContainer;

    [SerializeField]
    private EnemyData enemyData;
    private Invulnerable invulnerable;

    [SerializeField]
    private UnityEvent onDestruction;

    public bool isReflecting { get; set; } = true;

    public bool hasTotalReflection => true;

    private void Awake()
    {
        invulnerable = GetComponent<Invulnerable>();
        healthBar = GetComponent<HealthBar>();
    }

    private void OnEnable() {
        currentLifePoints = enemyData.maxLifePoints;
        healthBar.UpdateContent(currentLifePoints);
        healthBarContainer.SetActive(true);
    }

    public void TakeDamage(int damage)
    {
        if (invulnerable.isInvulnerable) return;

        currentLifePoints = Mathf.Clamp(
            currentLifePoints - damage,
            0,
            enemyData.maxLifePoints
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
