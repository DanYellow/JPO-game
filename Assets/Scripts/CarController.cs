using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;
using System;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody motor;

    [SerializeField]
    private Rigidbody collision;

    private BoxCollider boxCollider;

    [SerializeField]
    private GameObject[] listWheels;

    [SerializeField]
    private CarData carData;

    private RaycastHit hit;

    [SerializeField]
    private LayerMask groundLayers;

    private Vector3 moveInput = Vector3.zero;

    private float lastDirection = 1;
    private float groundDrag;

    [SerializeField]
    private Transform cameraTracker;

    [SerializeField]
    private Transform spawnMeteorPivotPoint;

    private float drifitngTimer = 0.75f;
    private float drifitngTimeRemaning = 0;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarSlowdown;

    [SerializeField]
    private BoolValue isCarGrounded;
    [SerializeField]
    private BoolValue isCarDrifting;

    private void OnEnable()
    {
        onCarSlowdown.OnEventRaised += IncreaseDrag;
    }

    void Awake()
    {
        groundDrag = carData.groundDrag;
        collision.useGravity = false;
        drifitngTimeRemaning = drifitngTimer;

        motor.transform.parent = null;
        collision.transform.parent = null;

        boxCollider = collision.GetComponent<BoxCollider>();
    }

    void Update()
    {
        ManageWheels();
        Rotate();
        SwapDrag();
        MoveSpawnMeteorPoint();

        collision.position = motor.position;
        transform.position = motor.transform.position;

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20 * Time.deltaTime);

        isCarDrifting.CurrentValue = IsDrifting();
        if(isCarDrifting.CurrentValue) {
            drifitngTimeRemaning -= Time.deltaTime;
        }
        // bool isDrifting = IsDrifting();
        // if (isDrifting && isCarDrifting.CurrentValue != isDrifting)
        // {
        //     print("Her");
        //     isCarDrifting.CurrentValue = IsDrifting();
        // }
    }

    private void MoveSpawnMeteorPoint()
    {
        Vector3 pos = boxCollider.bounds.center;
        pos.y = spawnMeteorPivotPoint.position.y;

        // pos += new Vector3(10, 0, 10);
        // Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * spawnMeteorPivotPoint.rotation;
        // spawnMeteorPivotPoint.rotation = Quaternion.Slerp(spawnMeteorPivotPoint.rotation, targetRotation, 1 * Time.deltaTime);
        // spawnMeteorPivotPoint.position = pos;
        Vector3 relativePos = spawnMeteorPivotPoint.position - transform.position;
        //  spawnMeteorPivotPoint.LookAt(transform, -Vector3.forward);
        // spawnMeteorPivotPoint.Rotate(Vector3.up);
        if (motor.velocity.sqrMagnitude > Mathf.Epsilon)
        {    // Where EPSILON is a very small number
            //  spawnMeteorPivotPoint.rotation = Quaternion.LookRotation(transform.forward * moveInput.y);
        }

        // spawnMeteorPivotPoint.rotation = Quaternion.FromToRotation(spawnMeteorPivotPoint.forward, tr.forward * -1);
    }

    private void IncreaseDrag()
    {
        groundDrag += carData.groundDrag * 0.0095f;
    }

    private void FixedUpdate()
    {
        isCarGrounded.CurrentValue = Physics.Raycast(transform.position, -transform.up, out hit, 1, groundLayers);
        carData.currentVelocity = motor.velocity.sqrMagnitude;

        if (isCarGrounded.CurrentValue)
        {
            float finalSpeed = carData.forwardSpeed;
            // finalSpeed *= Mathf.Abs(inputX) > 0 ? 0.95f : 1; moveInput.y
            motor.AddForce(finalSpeed * transform.forward * moveInput.y, ForceMode.Acceleration);
        }
        else
        {
            motor.AddForce(-transform.up * Physics.gravity.y);
        }
        motor.velocity = Vector3.ClampMagnitude(motor.velocity, 50);

        collision.MoveRotation(transform.rotation);


    }

    private bool IsDrifting()
    {
        float driftValue = Vector3.Dot(motor.velocity, motor.transform.forward);
        float driftAngle = Mathf.Acos(driftValue) * Mathf.Rad2Deg;

        bool _isDrifting = driftAngle <= 89f || driftAngle >= 91f;
        if(!_isDrifting && drifitngTimeRemaning > 0 && isCarDrifting.CurrentValue) {
            return true;
        }
        drifitngTimeRemaning = drifitngTimer;

        return _isDrifting;
    }

    private void Rotate()
    {
        if (motor.velocity.sqrMagnitude <= 15)
        {
            return;
        }

        float newRotation = carData.turnSpeed * moveInput.x * Time.deltaTime;
        if (isCarGrounded.CurrentValue)
        {
            transform.Rotate(0, newRotation, 0, Space.Self);
        }
    }

    private void SwapDrag()
    {
        if (isCarGrounded.CurrentValue)
        {
            motor.drag = groundDrag;
        }
        else
        {
            motor.drag = carData.airDrag;
        }
    }

    private void ManageWheels()
    {
        for (var i = 0; i < listWheels.Length; i++)
        {
            var wheel = listWheels[i];
            wheel.transform.Rotate(Time.deltaTime * moveInput.y * carData.rotationWheelSpeed, 0, 0, Space.Self);

            if (i < 2)
            {
                wheel.transform.parent.transform.localRotation = Quaternion.Euler(new Vector3(
                    wheel.transform.parent.transform.localRotation.x,
                    Mathf.Lerp(-carData.steerAngle, carData.steerAngle, moveInput.x * 0.5f + 0.5f),
                    wheel.transform.parent.transform.localRotation.z
                ));
            }
        }
    }

    public void OnDrive(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            StopAllCoroutines();
            moveInput = (Vector3)ctx.ReadValue<Vector2>();
            print(moveInput.normalized);
            if (lastDirection != moveInput.y)
            {
                lastDirection = moveInput.y;
            }
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            StopAllCoroutines();
            StartCoroutine(Glide());
        }
    }

    private IEnumerator Glide()
    {
        float current = 0;
        float duration = 0.08f;

        yield return null;
        while (current <= 1)
        {
            moveInput.y = Mathf.Lerp(1, 0, Mathf.Sin(current * Mathf.PI * 0.5f));

            current += Time.deltaTime / duration;

            yield return null;
        }

        moveInput.y = 0;
    }

    private void OnDisable()
    {
        onCarSlowdown.OnEventRaised -= IncreaseDrag;
    }
}
