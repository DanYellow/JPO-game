using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    // https://forum.unity.com/threads/how-can-i-cause-my-rigidbody2d-to-float-up-and-down-in-script.1243708/

    private Vector2 startingPosition;

    private void Awake()
    {
        startingPosition = transform.position;
    }

    private void Update()
    {
        float newY = (Mathf.Sin(Time.time * 5) * 0.2f) + startingPosition.y;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
