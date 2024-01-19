using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    // https://forum.unity.com/threads/how-can-i-cause-my-rigidbody2d-to-float-up-and-down-in-script.1243708/

    private Vector2 startingPosition;

    private Rigidbody2D rb;

    [SerializeField]
    private float floatStrength = 0.2f;
    [SerializeField]
    private float speed = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startingPosition = rb.position;
    }

    private void FixedUpdate() {
        // float newY = (Mathf.Sin(Time.time * speed) * floatStrength) + startingPosition.y;
        // Vector2 position = new Vector2(rb.position.x, newY);
        // rb.MovePosition(position);
    }

    public void SetStartingPosition(Vector2 pos) {
        startingPosition = pos;
    }
}
