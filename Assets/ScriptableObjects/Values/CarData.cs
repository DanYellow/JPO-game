using UnityEngine;

[CreateAssetMenu(fileName = "New Car Data", menuName = "ScriptableObjects/Values/CarData", order = 0)]
public class CarData : ScriptableObject
{
    public float forwardSpeed = 175;
    public float backwardSpeed = 105;
    public float turnSpeed = 500;
    public float rotationWheelSpeed = 500;
    public float steerAngle = 19.5f;
    public float groundDrag = 2.5f;
    public float airDrag = 2.5f;
    public float currentVelocity = 0;

    [HideInInspector]
    public bool isMovingBackward = false;
}
