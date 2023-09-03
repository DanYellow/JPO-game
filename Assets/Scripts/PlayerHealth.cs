using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private BoolEventChannel onHealthUpdated;

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private CameraShakeTypeValue hurtCameraShake;

    [SerializeField]
    private CameraShakeTypeValue deathCameraShake;

    [SerializeField]
    private PotionEventChannel onPotionPicked;

    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField]
    private VoidEventChannel onPlayerDeath;

    public GameObject deathEffectPrefab;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;


    private void Awake()
    {
        // playerStatsValue.nbCurrentLifes = playerStatsValue.nbMaxLifes;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        onPlayerDeath.OnEventRaised += OnDeath;
        onPotionPicked.OnEventRaised += OnHeal;

        // playerStatsValue.currentLifePoints = 50;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
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
            onPlayerDeath.Raise();
            OnDeath();
        }
        else
        {
            onCinemachineShake.Raise(hurtCameraShake);
        }
    }

    private void OnHeal(PotionValue potionTypeValue) {
        if(potionTypeValue.type == PotionType.Heal) {
            playerStatsValue.currentLifePoints = Math.Clamp(
                playerStatsValue.currentLifePoints + potionTypeValue.value,
                0,
                playerStatsValue.maxLifePoints
            );
            onHealthUpdated.Raise(false);
        }
    }

    private void OnDeath()
    {
        onCinemachineShake.Raise(deathCameraShake);
        // GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        // Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        // Destroy(gameObject);
    }

    private void OnDisable()
    {
        onPlayerDeath.OnEventRaised -= OnDeath;
        onPotionPicked.OnEventRaised -= OnHeal;
    }
}
