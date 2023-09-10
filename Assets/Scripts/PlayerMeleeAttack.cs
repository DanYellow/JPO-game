using UnityEngine.InputSystem;
using UnityEngine;
using System.Collections;

public class PlayerMeleeAttack : MonoBehaviour, IAttackable
{
    [SerializeField]
    private VoidEventChannel lightAttackEventChannel;

    [SerializeField]
    private BoolValue playerCanMove;

    // [HideInInspector]
    public bool isAttacking { get; set; } = false;

    public void OnLightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            isAttacking = true;
            lightAttackEventChannel.Raise();
            StartCoroutine(ResetAttackState());
        }
    }

    IEnumerator ResetAttackState()
    {
        yield return null;
        yield return new WaitForSeconds(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        isAttacking = false;
    }
}
