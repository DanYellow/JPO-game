using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private BoxCollider2D bc2d;
    private Animator animator;

    private void Awake() {
        bc2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerSpawn playerSpawn))
        {
            animator.SetTrigger(AnimationStrings.activate);
            playerSpawn.currentSpawnPosition = transform.position;
            bc2d.enabled = false;
        }
    }
}
