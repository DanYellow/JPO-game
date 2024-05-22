using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

// https://www.youtube.com/watch?v=DVHcOS1E5OQ

public class CarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody motor;

    [SerializeField]
    private Rigidbody collision;

    [SerializeField]
    private GameObject[] listWheels;

    private RaycastHit hit;

    [SerializeField]
    private LayerMask groundLayers;

    private Vector3 moveInput = Vector3.zero;

    private float lastDirection = 1;
    private float groundDrag;


    [SerializeField]
    private Transform driftPoint;

    [SerializeField]
    private float drifitngTimer = 0.85f;
    private float drifitngTimeRemaning = 0;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarSlowdown;

    [SerializeField]
    private BoolValue isCarGrounded;
    [SerializeField]
    private BoolValue isCarDrifting;

    [SerializeField]
    private CarData carData;

    [SerializeField]
    private VoidEventChannel onGameOver;

    private void OnEnable()
    {
        onCarSlowdown.OnEventRaised += IncreaseDrag;
        onGameOver.OnEventRaised += OnGameOver;
    }

    void Awake()
    {
        groundDrag = carData.groundDrag;
        collision.useGravity = false;
        drifitngTimeRemaning = drifitngTimer;

        motor.transform.parent = null;
        collision.transform.parent = null;
    }

    void Update()
    {
        ManageWheels();
        Rotate();
        SwapDrag();

        Quaternion targetRotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 20f * Time.deltaTime);
        // transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles)

        if (HasStartDrifting())
        {
            drifitngTimeRemaning -= Time.deltaTime;
            if (drifitngTimeRemaning <= 0)
            {
                isCarDrifting.CurrentValue = true;
            }
        }
        else
        {
            isCarDrifting.CurrentValue = false;
            drifitngTimeRemaning = drifitngTimer;
        }

        carData.isMovingBackward = moveInput.normalized.y < 0;
        collision.mass = 0;
        // collision.mass = Mathf.Abs(moveInput.normalized.y) > 0 ? 1 : 0;
    }

    private void LateUpdate()
    {
        // collision.position = motor.position;
        transform.position = motor.transform.position - new Vector3(0, 0.4f, 0);
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
            float finalSpeed = carData.forwardSpeed * 1.45f;
            motor.AddForce(finalSpeed * transform.forward * moveInput.y, ForceMode.Acceleration);
        }

        motor.AddForce(-transform.up * Physics.gravity.y, ForceMode.Acceleration);

        motor.velocity = Vector3.ClampMagnitude(motor.velocity, 50);
        collision.MoveRotation(transform.rotation);
        motor.MoveRotation(transform.rotation);
        collision.MovePosition(motor.position);
    }

    private bool HasStartDrifting()
    {
        return Mathf.Abs(moveInput.normalized.x) > 0 && Mathf.Abs(moveInput.normalized.y) > 0;
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
            transform.RotateAround(driftPoint.position, transform.up, newRotation);
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

            if (lastDirection != moveInput.y)
            {
                lastDirection = moveInput.y;
            }
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            moveInput.x = 0;
            if (lastDirection == 0)
            {
                return;
            }
            StopAllCoroutines();
            StartCoroutine(Glide());
        }
    }

    private IEnumerator Glide()
    {
        float current = 0;
        float duration = 0.1f;

        yield return null;
        while (current <= 1)
        {
            moveInput.y = Mathf.Lerp(1, 0, Mathf.Sin(current * Mathf.PI * 0.5f));

            current += Time.deltaTime / duration;

            yield return null;
        }

        moveInput.y = 0;
    }

    private void OnGameOver()
    {
        motor.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnDisable()
    {
        onCarSlowdown.OnEventRaised -= IncreaseDrag;
        onGameOver.OnEventRaised -= OnGameOver;
    }
}
