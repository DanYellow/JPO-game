using UnityEngine.InputSystem;
using UnityEngine;

public class WorldGridManager : MonoBehaviour
{
    [SerializeField]
    private Material render;

    private float baseGridSpeed;
    private float currentGridSpeed;
    private float startGridSpeed;
    private float targetSpeed;

    private float lastYDir;

    private float t = 0.0f;

    private Vector3 moveInput = Vector3.zero;

    void Start()
    {
        // 0.0875
        baseGridSpeed = render.GetFloat("_GridSpeed");
        startGridSpeed = baseGridSpeed;
    }

    void Update()
    {
        targetSpeed = startGridSpeed;
        if (moveInput.y < 0)
        {
            targetSpeed *= -1;
        }
        else if (moveInput.y == 0)
        {
            targetSpeed *= 0.5f;
        }

        if (startGridSpeed > targetSpeed)
        {
            currentGridSpeed = Mathf.Lerp(targetSpeed, startGridSpeed, t);
        }
        else
        {
            currentGridSpeed = Mathf.Lerp(startGridSpeed, targetSpeed, t);
        }

        render.SetFloat("_GridSpeed", currentGridSpeed);
        t += 0.85f * Time.deltaTime;

        if (currentGridSpeed == targetSpeed && lastYDir != moveInput.y)
        {
            t = 0.0f;
            lastYDir = moveInput.y;
            startGridSpeed = targetSpeed;
            currentGridSpeed = render.GetFloat("_GridSpeed");
        }
    }

    public void OnDrive(InputAction.CallbackContext ctx)
    {
        moveInput = (Vector3)ctx.ReadValue<Vector2>();

    }

    private void OnDisable()
    {
        render.SetFloat("_GridSpeed", baseGridSpeed);
    }
}
