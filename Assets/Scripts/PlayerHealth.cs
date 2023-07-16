using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private CameraShakeTypeValue hurtCameraShake;

    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    public GameObject deathEffectPrefab;

    [SerializeField]
    private PlayerStatsValue playerStatsValue;

    public bool isInvulnerable { get; set; } = false;

    private void Awake()
    {
        playerStatsValue.nbCurrentLifes = playerStatsValue.nbMaxLifes;
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised += OnDeath;
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage();
        }
#endif
    }

    // Update is called once per frame
    public void TakeDamage()
    {
        if (isInvulnerable) return;

        playerStatsValue.nbCurrentLifes = Mathf.Clamp(
            playerStatsValue.nbCurrentLifes - 1,
            0,
            playerStatsValue.nbMaxLifes
        );

        isHurtVoidEventChannel.Raise();
        if (playerStatsValue.nbCurrentLifes == 0)
        {
            onPlayerDeathVoidEventChannel.Raise();
        }
        else
        {
            Debug.Log("onCinemachineShake");
            onCinemachineShake.Raise(hurtCameraShake);
        }
    }

    private void OnDeath()
    {
        GameObject deathEffect = Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        Destroy(deathEffect, deathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= OnDeath;

    }
}
