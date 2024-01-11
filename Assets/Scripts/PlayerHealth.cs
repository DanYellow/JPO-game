using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private BoolEventChannel onHealthUpdated;

    [SerializeField]
    private VoidEventChannel onPlayerDeath;

    public GameObject deathEffectPrefab;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    [SerializeField]
    private UnityEvent onDeathEvent;

    private Invulnerable invulnerable;

    [Space(10)]

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private CameraShakeTypeValue hurtCameraShake;
    [SerializeField]
    private CameraShakeTypeValue deathCameraShake;

    private void Awake()
    {
        // playerStatsValue.nbCurrentLifes = playerStatsValue.nbMaxLifes;
        // onPlayerDeath.OnEventRaised += OnDeath;

        invulnerable = GetComponent<Invulnerable>();


        playerStatsValue.currentLifePoints = 1;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(int.MaxValue);
        }
#endif
    }

    // Update is called once per frame
    public void TakeDamage(int damage)
    {
        playerStatsValue.currentLifePoints = Mathf.Clamp(
            playerStatsValue.currentLifePoints - damage,
            0,
            playerStatsValue.maxLifePoints
        );

        onHealthUpdated.Raise(true);
        if (playerStatsValue.currentLifePoints <= 0)
        {
            Death();
        }
        else
        {
            onCinemachineShake.Raise(hurtCameraShake);
            invulnerable.Trigger();
        }
    }

    public void Heal(PotionValue potionTypeValue) {
        if(potionTypeValue.type == PotionType.Heal) {
            int newPointsLife = Mathf.Clamp(
                playerStatsValue.currentLifePoints + potionTypeValue.value,
                0,
                playerStatsValue.maxLifePoints
            );
            playerStatsValue.currentLifePoints = newPointsLife;
            onHealthUpdated.Raise(false);
        }
    }

    private void Death()
    {
        print("Death");
        onDeathEvent?.Invoke();
        onPlayerDeath?.Raise();
        onCinemachineShake.Raise(deathCameraShake);
        // GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        // Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        // Destroy(gameObject);
    }

    private void OnDisable()
    {
        // onPlayerDeath.OnEventRaised -= OnDeath;
    }
}
