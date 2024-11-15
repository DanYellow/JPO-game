using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;

    [SerializeField]
    private VoidEventChannel onPositionSetEvent;

    private void Start()
    {
        GetComponent<Rigidbody>().position = new Vector3(
            startPosition.position.x,
            transform.position.y,
            startPosition.position.z
        );
    }

    private void OnEnable()
    {
        onPositionSetEvent.OnEventRaised += OnPositionSet;
    }

    private void OnPositionSet()
    {
        GetComponentInChildren<Light>().transform.parent = null;
    }

    private void OnDisable()
    {
        onPositionSetEvent.OnEventRaised -= OnPositionSet;
    }
}
