using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel resetPlayerPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(1);

            PlayerHealth playerHealth = other.gameObject.GetComponentInChildren<PlayerHealth>();

            if (other.gameObject.TryGetComponent(out PlayerSpawn playerSpawn) ) // && playerHealth.GetHealth() > 0
            {
                other.transform.position = playerSpawn.currentSpawnPosition;
                // We "stop" the player on respawn
                other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                resetPlayerPosition.Raise();
            }

            IStunnable iStunnable = other.transform.GetComponent<IStunnable>();
            StartCoroutine(iStunnable.Stun(2, () => {}));
        }
        else if (other.transform.parent == null)
        {
            Destroy(other.gameObject);
        }
    }
}