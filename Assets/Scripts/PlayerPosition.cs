using System.Collections;
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

        StartCoroutine(UnparentLight());
    }

    private IEnumerator UnparentLight() {
        yield return null;
        GetComponentInChildren<Light>().transform.parent = null;
    }

    private void OnEnable()
    {
        onPositionSetEvent.OnEventRaised += OnPositionSet;
    }

    private void OnPositionSet()
    {
    }

    private void OnDisable()
    {
        onPositionSetEvent.OnEventRaised -= OnPositionSet;
    }
}
