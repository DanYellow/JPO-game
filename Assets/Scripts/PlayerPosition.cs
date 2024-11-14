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
    }
}
