using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityBody : MonoBehaviour
{
    public GravityAttractor planet;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        // Disable rigidbody gravity and rotation as this is simulated in GravityAttractor script
        rb.useGravity = false;
        // https://forum.unity.com/threads/faux-gravity-spinning-at-south-pole.180580/
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        // rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
    

    void FixedUpdate()
    {
        // Allow this body to be influenced by planet's gravity
        planet.Attract(rb);
    }
}
