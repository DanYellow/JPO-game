using UnityEngine;

public class ComputeDistanceTravelled : MonoBehaviour
{
    private Vector3 lastPosition;
    private FloatValue totalDistance;
    [SerializeField]
    private FloatValue distanceTravelled;

    private void Start()
    {
        distanceTravelled.CurrentValue = 0;
        lastPosition = transform.position;
    }

    private void Update()
    {
        distanceTravelled.CurrentValue += SphericalDistance(lastPosition, transform.position);

        lastPosition = transform.position;
    }

    float SphericalDistance(Vector3 position1, Vector3 position2)
    {
        return Vector3.Distance(position1, position2);
    }
}
