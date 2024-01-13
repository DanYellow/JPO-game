using System.Collections;
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

    [SerializeField, UnityEngine.Serialization.FormerlySerializedAs("onDeathEvent")] 
    private UnityEvent onDeathUnityEvent;

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
        StartCoroutine(ActivateKinematic());
    }

    IEnumerator ActivateKinematic()
    {
        Animator animator = GetComponent<Animator>();
        yield return new WaitUntil(() => animator.GetBool(AnimationStrings.isDead) == true);
        yield return new WaitWhile(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1);
        yield return new WaitUntil(() => GetComponentInParent<PlayerMovements>().IsGrounded() == true);
        onDeathUnityEvent?.Invoke();
    }

    public int GetHealth()
    {
        return playerStatsValue.currentLifePoints;
    }
}
