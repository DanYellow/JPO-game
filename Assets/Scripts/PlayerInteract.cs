using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onInteractEventChannel;

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            onInteractEventChannel.Raise();
        }
    }
}
