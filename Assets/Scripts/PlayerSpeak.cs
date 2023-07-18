using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerSpeak : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel playerListenEventChannel;

    public void OnSpeak(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            playerListenEventChannel.Raise();
        }
    }
}
