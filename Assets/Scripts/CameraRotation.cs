using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraRotation : MonoBehaviour
{
    public Transform target;

    private Vector3 moveInput = Vector3.zero;

    private Quaternion originalRot;

    private float timeCount = 0.0f;


    private void Start()
    {
        originalRot = transform.rotation;
    }

    // Update is called once per frame -transform.forward
    void Update()
    {
        // transform.rotation = Quaternion.FromToRotation(new Vector3( 
        //     -transform.forward.x,
        //     -transform.forward.y,
        //     -transform.forward.z
        // ), target.position) * transform.rotation;

        if (true) // moveInput.x == 0
        {
            timeCount = 0;
            // transform.rotation = originalRot;
            transform.rotation = Quaternion.FromToRotation(
                -transform.forward,
                target.position
            ) * transform.rotation;
        }
        else
        {
            // transform.rotation = Quaternion.Slerp(
            //     originalRot,
            //     Quaternion.Euler(45, 60, 0),
            //     timeCount
            // );
            // timeCount = timeCount + Time.deltaTime;
        }
    }

    public void OnTurn(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }

    public static Vector3 SmoothDampEuler(Vector3 current, Vector3 target, ref Vector3 currentVelocity, float smoothTime)
    {
        return new Vector3(
          Mathf.SmoothDampAngle(current.x, target.x, ref currentVelocity.x, smoothTime),
          Mathf.SmoothDampAngle(current.y, target.y, ref currentVelocity.y, smoothTime),
          Mathf.SmoothDampAngle(current.z, target.z, ref currentVelocity.z, smoothTime)
        );
    }
}
