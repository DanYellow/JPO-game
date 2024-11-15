using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffectCollision : MonoBehaviour
{
    [SerializeField, Range(2, 10)]
    private float speed = 7;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            Vector3 dirX = (other.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(dirX); // other.ClosestPoint(transform.position)
        }

        gameObject.SetActive(false);
    }

    private void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
    }

    private void OnBecameInvisible()
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        StartCoroutine(Invincible());
    }

    private IEnumerator Invincible()
    {
        yield return Helpers.GetWait(0.5f);
        gameObject.SetActive(false);
    }
}
