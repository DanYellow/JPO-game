using UnityEngine;

public class ComputeDistanceTravelled : MonoBehaviour
{
    private Vector3 lastPosition;
    private FloatValue totalDistance;
    [SerializeField]
    private FloatValue distanceTravelled;

    [SerializeField]
    private int scoreStepThreshold = 850;

    [SerializeField]
    private int startSpawnObstaclesThreshold = 150;

    private float lastThousandth = 0;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onScoreThresholdReached;

    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    [SerializeField]
    private BoolValue isCarGrounded;

    private void Start()
    {
        lastPosition = transform.position;
        ResetScore();
    }

    public void ResetScore()
    {
        distanceTravelled.CurrentValue = 0;
    }

    private void Update()
    {
        if (isCarGrounded.CurrentValue)
        {
            distanceTravelled.CurrentValue += SphericalDistance(lastPosition, transform.position);
        }
        hasReachMinimumTravelDistance.CurrentValue = distanceTravelled.CurrentValue >= startSpawnObstaclesThreshold;

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
