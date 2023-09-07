using UnityEngine;

public class EnemyFlying : MonoBehaviour
{
    [SerializeField]
    private LayerMask targetLayerMask;

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
