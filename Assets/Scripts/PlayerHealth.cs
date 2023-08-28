using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField]
    private VoidEventChannel onHealthUpdated;

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
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public GameObject deathEffectPrefab;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;


    private void Awake()
    {
        // playerStatsValue.nbCurrentLifes = playerStatsValue.nbMaxLifes;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        onPlayerDeathVoidEventChannel.OnEventRaised += OnDeath;
        onPotionPicked.OnEventRaised += OnHeal;
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
            playerStatsValue.currentLifePoints - 1,
            0,
            playerStatsValue.maxLifePoints
        );

        onHealthUpdated.Raise();
        if (playerStatsValue.currentLifePoints <= 0)
        {
            onPlayerDeathVoidEventChannel.Raise();
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
            onHealthUpdated.Raise();
        }
    }

    private void OnDeath()
    {
        onCinemachineShake.Raise(deathCameraShake);
        GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= OnDeath;
        onPotionPicked.OnEventRaised -= OnHeal;
    }
}
