using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private VectorEventChannel onPlayerExit;
    

    [SerializeField]
    private int nbLives = 0;

    private Rigidbody rb;

    private Animator animator;

    private PlayerInvincibility playerInvincibility;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerInvincibility = GetComponent<PlayerInvincibility>();

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
    }

    private void OnBecameInvisible()
    {
        print("ggrgrgr");
        if (nbLives == 0)
        {
            onPlayerExit.OnEventRaised(transform.position);
        }
    }
}
