using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onInteractEventChannel;

    [SerializeField]
    private VoidEventChannel onCancelInteractEventChannel;

    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            onInteractEventChannel.Raise();
        }
        else if (ctx.phase == InputActionPhase.Canceled)
        {
            onCancelInteractEventChannel.Raise();
        }
    }
}
