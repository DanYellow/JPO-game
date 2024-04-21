using System.Collections;
using System.Collections.Generic;
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

    // Update is called once per frame -transform.forward
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, delay * Time.deltaTime);

        target.localRotation = Quaternion.Euler(new Vector3(
            target.localRotation.x,
            Mathf.Lerp(carData.steerAngle * 1.5f, -carData.steerAngle * 1.5f, moveInput.x * 0.5f + 0.5f),
            target.localRotation.z
        ));
    }

    public void OnTurn(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
