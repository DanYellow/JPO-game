using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerMeleeAttack : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel lightAttackEventChannel;

    [SerializeField]
    private LayerMask listDamageableLayers;

    [SerializeField]
    private Transform attackPoint;

    [SerializeField]
    private float attackRange = 0.5f;

    [SerializeField]
    private float lightAttackRate;
    private float nextLightAttackTime = 0;

    [SerializeField]
    private BoolValue playerCanMove;


    // [HideInInspector]
    public bool isAttacking = false;

    public void OnLightAttack(InputAction.CallbackContext ctx)
    {
        if (ctx.phase == InputActionPhase.Performed)
        {
            lightAttackEventChannel.Raise();
            playerCanMove.CurrentValue = false;
            Collider2D[] listHitItems = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, listDamageableLayers);

            foreach (var hitItem in listHitItems)
            {
                if (hitItem.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
                {
                    iDamageable.TakeDamage(1);
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void OnAttackEnds() {
        GetComponent<Animator>().SetBool(AnimationStrings.lightAttack, false);
        playerCanMove.CurrentValue = true;
    }
}
