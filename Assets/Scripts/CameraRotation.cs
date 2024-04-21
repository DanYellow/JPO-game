using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraRotation : MonoBehaviour
{
    public Transform target;

    [SerializeField]
    private float delay = 150;

    private Vector3 moveInput = Vector3.zero;

    // Update is called once per frame -transform.forward
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, delay * Time.deltaTime);
    }

    public void OnTurn(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
