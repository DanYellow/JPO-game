using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField]
    private Transform pivotPoint;

    [SerializeField]
    private float speed = 15;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(pivotPoint.position, new Vector3(0, 1, 0), speed * Time.deltaTime);
    }
}
