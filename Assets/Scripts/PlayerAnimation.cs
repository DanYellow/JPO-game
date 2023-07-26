using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    [SerializeField]
    private VectorEventChannel vectorEventChannel;

    // https://forum.unity.com/threads/unsubscribe-from-an-event-using-a-lambda-expression.1287587/
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        vectorEventChannel.OnEventRaised += UpdateMovement;
    }

    private void UpdateMovement(Vector3 direction)
    {
        animator.SetFloat("VelocityX", Mathf.Abs(direction.x));
    }

    private void OnDisable()
    {
        vectorEventChannel.OnEventRaised -= UpdateMovement;
    }
}
