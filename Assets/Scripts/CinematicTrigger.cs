using UnityEngine.Playables;
using UnityEngine;

public class CinematicTrigger : MonoBehaviour
{
    private PlayableDirector director;
    private new Collider2D collider;

    private void Awake()
    {
        director = GetComponent<PlayableDirector>();
        collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Vector3 distance = other.transform.position - transform.position;
        if (
            other.CompareTag("Player") &&
            distance.x < 0
        )
        {
            collider.enabled = false;
            director.Play();
        }
    }
}
