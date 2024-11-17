using UnityEngine;
using UnityEngine.Events;

public class WaveButton : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private UnityEvent<Transform> onPositionSet;

    [SerializeField]
    private Transform playerPosition;

    private void Start()
    {
        transform.position = new Vector3(
            startPosition.position.x,
            startPosition.position.y,
            startPosition.position.z
        );

        onPositionSet.Invoke(playerPosition);
    }
}
