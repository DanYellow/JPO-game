using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    private BoxCollider2D bc2d;
    private Animator animator;
    private new Light2D light;

    [SerializeField]
    private Vector2Value lastCheckpointPosition;

    private void Awake()
    {
        bc2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        light = GetComponentInChildren<Light2D>(true);
        light.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision.TryGetComponent(out PlayerSpawn playerSpawn))
        {
            animator.SetTrigger(AnimationStrings.activate);
            playerSpawn.currentSpawnPosition = transform.position;
            lastCheckpointPosition.CurrentValue = transform.position;
            bc2d.enabled = false;
            light.enabled = true;
        }
    }
}
