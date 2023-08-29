using UnityEngine;
using UnityEngine.Events;


public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private VectorEventChannel rbVelocityEventChannel;

    [SerializeField]
    private VoidEventChannel lightAttackEventChannel;

    [SerializeField]
    private BoolValue playerIsDashing;


    private UnityAction onLightAttackEvent;

    // https://forum.unity.com/threads/unsubscribe-from-an-event-using-a-lambda-expression.1287587/
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        rbVelocityEventChannel.OnEventRaised += UpdateMovement;

        onLightAttackEvent = () =>
        {
            if(!playerIsDashing.CurrentValue) {
                // animator.SetTrigger("LightAttack2");
                animator.SetBool(AnimationStrings.lightAttack, true);
            }
        };
        lightAttackEventChannel.OnEventRaised += onLightAttackEvent;
    }

    private void UpdateMovement(Vector3 direction)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(direction.x));
        animator.SetFloat("VelocityY", direction.y);
    }

    private void OnDisable()
    {
        rbVelocityEventChannel.OnEventRaised -= UpdateMovement;
        lightAttackEventChannel.OnEventRaised -= onLightAttackEvent;
    }
}
