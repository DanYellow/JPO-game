using UnityEngine.InputSystem;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody motor;

    [SerializeField]
    private Rigidbody collision;

    [SerializeField]
    private GameObject[] listWheels;

    [SerializeField]
    private CarData carData;

    private RaycastHit hit;
    private bool isGrounded = true;
    [SerializeField]
    private LayerMask groundLayers;

    private Vector3 moveInput = Vector3.zero;

    void Awake()
    {
        motor.transform.parent = null;

        collision.useGravity = false;
        collision.transform.parent = null;
    }

    // Update is called once per frame
    void Update()
    {
        ManageWheels();
        Rotate();
        SwapDrag();

        transform.position = motor.transform.position;
        transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
    }

    private void FixedUpdate()
    {
        isGrounded = Physics.Raycast(transform.position, -transform.up, out hit, 1, groundLayers);
        if (isGrounded)
        {
            float finalSpeed = carData.forwardSpeed;
            // finalSpeed *= Mathf.Abs(inputX) > 0 ? 0.95f : 1; moveInput.y
            motor.AddForce(finalSpeed * transform.forward * moveInput.y, ForceMode.Acceleration);
        }
        else
        {
            motor.AddForce(-transform.up * Physics.gravity.y);
        }

        collision.MoveRotation(transform.rotation);
    }

    private void Rotate()
    {
        float newRotation = carData.turnSpeed * moveInput.x * Time.deltaTime;
        if (isGrounded)
        {
            transform.Rotate(0, newRotation * moveInput.y, 0, Space.Self);
        }
    }

    private void SwapDrag()
    {
        if (isGrounded)
        {
            motor.drag = carData.groundDrag;
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
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
