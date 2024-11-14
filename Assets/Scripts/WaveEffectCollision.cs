using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffectCollision : MonoBehaviour
{
    [SerializeField, Range(2, 10)]
    private float speed = 7;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
        {
            Debug.Log(other.transform.name);
            Vector3 dirX = (other.transform.position - transform.position).normalized;
            //  rigidbody.AddExplosionForce(1000, other.ClosestPoint(transform.position), 10, 0);
            rigidbody.AddForce(dirX * 50, ForceMode.VelocityChange);
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }
}
