using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    [field:SerializeField]
    public bool isGrounded { private set; get; } = false;

    [SerializeField]
    private LayerMask listGroundLayers;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private float groundCheckRadius;

    private void FixedUpdate()
    {
        isGrounded = _IsGrounded();
    }

    private bool _IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, listGroundLayers);
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
