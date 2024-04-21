using UnityEngine;
using UnityEngine.InputSystem;


public class CameraRotation : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float delay = 150;

    [SerializeField]
    private CarData carData;

    private Vector3 moveInput = Vector3.zero;

    private float steerFactor = 1.25f;

    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, delay * Time.deltaTime);

        target.localRotation = Quaternion.Euler(new Vector3(
            target.localRotation.x,
            Mathf.Lerp(carData.steerAngle * steerFactor, -carData.steerAngle * steerFactor, moveInput.x * 0.5f + 0.5f),
            target.localRotation.z
        ));
    }

    public void OnTurn(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
