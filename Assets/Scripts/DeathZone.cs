using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel resetPlayerPosition;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (other.gameObject.TryGetComponent(out IDamageable iDamageable))
            {
                iDamageable.TakeDamage(1);
            }
            if (other.gameObject.TryGetComponent(out PlayerSpawn playerSpawn))
            {
                other.transform.position = playerSpawn.currentSpawnPosition;
                // We "stop" the player on respawn
                other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                resetPlayerPosition.Raise();
            }
        }
        else if (other.transform.parent == null)
        {
            Destroy(other.gameObject);
        }
    }
}