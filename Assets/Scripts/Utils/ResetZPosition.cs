using UnityEngine;

public class ResetZPosition : MonoBehaviour
{
    private void Awake()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    // void Update()
    // {
    //     Vector3 pos = transform.position;
    //     pos.z = 0;
    //     transform.position = pos;
    // }

    private void OnEnable()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
