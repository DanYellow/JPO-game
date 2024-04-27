using UnityEngine;

public static class Extensions
{
    public static void AddForceAtAngle(this Rigidbody rb, float force, float angle)
    {
        float xcomponent = Mathf.Cos(angle * Mathf.PI / 180) * force;
        float ycomponent = Mathf.Sin(angle * Mathf.PI / 180) * force;

        rb.AddForce(ycomponent, 0, xcomponent);
    }
}