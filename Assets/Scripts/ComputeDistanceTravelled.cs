using UnityEngine;

public class ComputeDistanceTravelled : MonoBehaviour
{
    private Vector3 lastPosition;
    private FloatValue totalDistance;
    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private int scoreStepThreshold = 850;

    private float lastThousandth = 0;

    [SerializeField]
    private VoidEventChannel onScoreThresholdReached;

    private void Start()
    {
        distanceTravelled.CurrentValue = 0;
        lastPosition = transform.position;
    }

    private void Update()
    {
        distanceTravelled.CurrentValue += SphericalDistance(lastPosition, transform.position);

        lastPosition = transform.position;

        float thousandth = Mathf.Floor(distanceTravelled.CurrentValue / scoreStepThreshold);
        if (thousandth >= 1 && thousandth > lastThousandth)
        {
            lastThousandth = thousandth;
            onScoreThresholdReached.Raise();
        }
    }

    float SphericalDistance(Vector3 position1, Vector3 position2)
    {
        return Vector3.Distance(position1, position2);
    }
}
