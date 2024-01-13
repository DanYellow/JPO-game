using UnityEngine;

public class ResetZPosition : MonoBehaviour
{
    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
