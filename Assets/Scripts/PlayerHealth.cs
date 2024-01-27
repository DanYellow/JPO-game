using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private BoolEventChannel onHealthUpdated;

    [SerializeField]
    private VoidEventChannel onPlayerDeath;

    public GameObject deathEffectPrefab;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    private Invulnerable invulnerable;

    [SerializeField]
    private LayerMask layerAfterDeath;

    [Space(10)]

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private CameraShakeTypeValue hurtCameraShake;
    [SerializeField]
    private CameraShakeTypeValue deathCameraShake;

    [SerializeField]
    private int testStartHealthPoints = 20;

    private void Awake()
    {
        // playerStatsValue.nbCurrentLifes = playerStatsValue.nbMaxLifes;
        invulnerable = GetComponent<Invulnerable>();
        #if UNITY_EDITOR
        playerStatsValue.currentLifePoints = testStartHealthPoints;
        #endif
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
        if (playerStatsValue.currentLifePoints == 0)
        {
            return;
        }
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

    public void Heal(PotionValue potionTypeValue)
    {
        if (potionTypeValue.type == PotionType.Heal)
        {
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
        onPlayerDeath?.Raise();
        onCinemachineShake.Raise(deathCameraShake);
        gameObject.layer = Helpers.GetLayerIndex(layerAfterDeath.value);
        transform.parent.gameObject.layer = Helpers.GetLayerIndex(layerAfterDeath.value);
    }

    public int GetHealth()
    {
        return playerStatsValue.currentLifePoints;
    }
}
