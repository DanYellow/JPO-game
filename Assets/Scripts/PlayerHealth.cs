using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private VectorEventChannel onPlayerExit;
    
    [SerializeField]
    private VoidEventChannel onPlayerDeathEvent;

    [SerializeField]
    private int nbLives = 0;

    private Rigidbody rb;

    private Animator animator;

    private PlayerInvincibility playerInvincibility;

    private Light lightLandmark;

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

    public void TakeDamage(Vector3 impactPoint)
    {
        nbLives--;

        if (nbLives == 0)
        {
            onPlayerExit.OnEventRaised(transform.position);
            Die(impactPoint);
        }
        else
        {
            StartCoroutine(playerInvincibility.Invincible());
            animator.SetTrigger(AnimationStrings.isHit);
        }
    }

    private void Die(Vector3 impactPoint)
    {
        rb.AddForce(impactPoint * 35, ForceMode.VelocityChange);
        lightLandmark.enabled = false;
        onPlayerDeathEvent.OnEventRaised();
        OnDeath.Invoke();
    }

    private void OnBecameInvisible()
    {
        if (nbLives == 0)
        {
            onPlayerExit.OnEventRaised(transform.position);
        }
    }
}
