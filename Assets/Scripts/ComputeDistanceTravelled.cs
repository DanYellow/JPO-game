using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputeDistanceTravelled : MonoBehaviour
{
    private Vector3 lastPosition;
    private float totalDistance;
    [SerializeField]
    private FloatValue distanceTravelled;

    private void Start()
    {
        distanceTravelled.CurrentValue = 0;
        lastPosition = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(lastPosition, transform.position);
        distanceTravelled.CurrentValue += (float)System.Math.Round(distance, 2);

        lastPosition = transform.position;
    }

}
