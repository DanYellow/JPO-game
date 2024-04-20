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
        float distance = Mathf.Abs((lastPosition - transform.position).z);
        
        distanceTravelled.CurrentValue += distance;

        lastPosition = transform.position;
    }

}
