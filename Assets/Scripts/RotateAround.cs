using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField]
    private Transform pivotPoint;

    [SerializeField]
    private float speed = 15;

    void Update()
    {
        transform.RotateAround(pivotPoint.position, pivotPoint.up, speed * Time.deltaTime);
    }
}
