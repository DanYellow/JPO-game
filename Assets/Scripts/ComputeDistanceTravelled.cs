using UnityEngine;

public class ComputeDistanceTravelled : MonoBehaviour
{
    private Vector3 lastPosition;

    [SerializeField]
    private int scoreStepThreshold = 850;

    [SerializeField]
    private int startSpawnObstaclesThreshold = 150;

    [SerializeField]
    private int startSpawnStalagmitesThreshold = 1150;
    [SerializeField]
    private int startWorldShrinkThreshold = 2200;

    private float lastThousandth = 0;

    private bool isGameFinished = false;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onScoreThresholdReached;
    [SerializeField]
    private VoidEventChannel onGameOver;

    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    [SerializeField]
    private BoolValue hasReachMinimumTravelDistanceForStalagmites;
    [SerializeField]
    private BoolValue hasReachMinimumTravelDistanceForShrink;

    [SerializeField]
    private BoolValue isCarGrounded;

    [SerializeField]
    private FloatValue distanceTravelled;

    private void Start()
    {
        hasReachMinimumTravelDistance.CurrentValue = false;
        hasReachMinimumTravelDistanceForStalagmites.CurrentValue = false;

        lastPosition = transform.position;
        isGameFinished = false;
        ResetScore();
    }

    private void OnEnable()
    {
        onGameOver.OnEventRaised += GameOver;
    }

    public void ResetScore()
    {
        distanceTravelled.CurrentValue = 0;
    }

    private void Update()
    {
        if (isCarGrounded.CurrentValue && !isGameFinished)
        {
            distanceTravelled.CurrentValue += SphericalDistance(lastPosition, transform.position);
        }
        hasReachMinimumTravelDistance.CurrentValue = distanceTravelled.CurrentValue >= startSpawnObstaclesThreshold;
        hasReachMinimumTravelDistanceForStalagmites.CurrentValue = distanceTravelled.CurrentValue >= startSpawnStalagmitesThreshold;
        hasReachMinimumTravelDistanceForShrink.CurrentValue = distanceTravelled.CurrentValue >= startWorldShrinkThreshold;

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

    private void GameOver()
    {
        isGameFinished = true;
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= GameOver;
    }
}
