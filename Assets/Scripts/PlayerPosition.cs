using System.Collections;
using UnityEngine;

public class PlayerPosition : MonoBehaviour
{
    [SerializeField]
    private Transform startPosition;

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
}
