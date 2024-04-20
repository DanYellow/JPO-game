using UnityEngine;

public class GravityAttractor : MonoBehaviour
{
    public float gravity = -9.81f;

    public void Attract(Rigidbody body) {
        Vector3 gravityUp = (body.position - transform.position).normalized;
		Vector3 localUp = body.transform.up;

        // Align bodies up axis with the centre of planet
		Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;
        // body.rotation = Quaternion.Slerp(body.rotation, targetRotation, 50 * Time.deltaTime);
		body.rotation = Quaternion.FromToRotation(localUp, gravityUp) * body.rotation;

		// Apply downwards gravity to body
		body.AddForce(gravityUp * Physics.gravity.y * 10);
		// body.AddForce(gravityUp * gravity * body.mass);
		
    }
}
