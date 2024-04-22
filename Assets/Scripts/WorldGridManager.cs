using UnityEngine.InputSystem;
using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    [SerializeField]
    private Material render;

    private float gridSpeed;

    private Vector3 moveInput = Vector3.zero;

    void Start()
    {
        gridSpeed = render.GetFloat("_GridSpeed");
    }

    void Update()
    {
        float speed = gridSpeed;
        if(moveInput.y < 0) {
            speed *= -1;
        } else if (moveInput.y == 0) {
            speed *= 0.5f;
        }

        render.SetFloat("_GridSpeed", speed);
    }

    public void OnDrive(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();
    }
}
