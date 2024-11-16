using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private VectorEventChannel onPlayerExit;

    [SerializeField]
    private GameObjectEventChannel onPlayerDeathEvent;

    [SerializeField]
    private VoidEventChannel onGameEndEvent;

    [SerializeField]
    private int nbLives = 0;

    private Rigidbody rb;

    private Animator animator;

    private PlayerInvincibility playerInvincibility;

    private Light lightLandmark;

    private bool hasTriggeredExitScreenEvent = false;

    [SerializeField]
    private UnityEvent OnDeath;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerInvincibility = GetComponent<PlayerInvincibility>();

        lightLandmark = GetComponentInChildren<Light>();

        nbLives = playerData.root.maxNbLives;
    }

    private void OnEnable()
    {
        onGameEndEvent.OnEventRaised += OnGameEnd;
    }

    private void OnGameEnd()
    {
        if (nbLives > 0)
        {
            StartCoroutine(playerInvincibility.Invincible(10));
        }
    }

    public void TakeDamage(Vector3 impactPoint)
    {
        nbLives--;

        if (nbLives == 0)
        {
            Die(impactPoint);
        }
        else
        {
            StartCoroutine(playerInvincibility.Invincible(null));
            animator.SetTrigger(AnimationStrings.isHit);
        }
    }

    private void Update()
    {
        if (nbLives == 0 && !hasTriggeredExitScreenEvent)
        {
            var pos = Camera.main.WorldToScreenPoint(transform.position);
            bool isOffscreen = pos.x <= 0 || pos.x >= Screen.width ||
                pos.y <= 0 || pos.y >= Screen.height;

            if (isOffscreen)
            {
                hasTriggeredExitScreenEvent = true;
                onPlayerExit.OnEventRaised(transform.position);
                gameObject.SetActive(false);
            }
        }
    }

    private void Die(Vector3 impactPoint)
    {
        rb.AddForce(impactPoint * 60, ForceMode.VelocityChange);
        lightLandmark.enabled = false;
        onPlayerDeathEvent.OnEventRaised(gameObject);
        OnDeath.Invoke();
    }

    private void OnDisable()
    {
        onGameEndEvent.OnEventRaised += OnGameEnd;
    }
}
