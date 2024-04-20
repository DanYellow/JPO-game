using UnityEngine.InputSystem;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    [SerializeField]
    private GameObject skidMarks;

    private Vector3 moveInput = Vector3.zero;


    void Update()
    {
        ToggleSkidMarks(Mathf.Abs(moveInput.y) > 0);
    }
    
    private void ToggleSkidMarks(bool isEmitting)
    {
        foreach (Transform item in skidMarks.transform)
        {
            item.GetComponent<TrailRenderer>().emitting = isEmitting;
        }
    }

    public void OnDrive(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
