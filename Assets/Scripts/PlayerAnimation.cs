using UnityEngine;
using UnityEngine.Events;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private VectorEventChannel vectorEventChannel;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    [SerializeField]
    private VoidEventChannel isHurtVoidEventChannel;

    private UnityAction<bool> onLandEvent;

    private UnityAction onHurtEvent;
    private UnityAction onDeathEvent;
    // https://forum.unity.com/threads/unsubscribe-from-an-event-using-a-lambda-expression.1287587/
    private void Start()
    {
        animator = GetComponent<Animator>();

        onHurtEvent = () =>
        {
        };

        onDeathEvent = () =>
        {
        };
    }

    private void OnEnable()
    {
        vectorEventChannel.OnEventRaised += UpdateMovement;
        isHurtVoidEventChannel.OnEventRaised += onHurtEvent;
        onPlayerDeathVoidEventChannel.OnEventRaised += onDeathEvent;
    }

    private void UpdateMovement(Vector3 direction)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(direction.x));
    }

    private void OnDisable()
    {
        vectorEventChannel.OnEventRaised -= UpdateMovement;
        // jumpBoolEventChannel.OnEventRaised -= onJumpEvent;
        // isShootingEventChannel.OnEventRaised -= onShootEvent;
        // fallingBoolEventChannel.OnEventRaised -= onFallEvent;
        // isGroundedBoolEventChannel.OnEventRaised -= onLandEvent;
        isHurtVoidEventChannel.OnEventRaised -= onHurtEvent;
        onPlayerDeathVoidEventChannel.OnEventRaised -= onDeathEvent;
    }
}
