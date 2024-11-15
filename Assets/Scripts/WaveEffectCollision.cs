using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveEffectCollision : MonoBehaviour
{
    [SerializeField, Range(2, 10)]
    private float speed = 7;

    private ObjectPooled objectPooled;

    private Coroutine autoDestroyCoroutine;

    private void Awake()
    {
        objectPooled = GetComponent<ObjectPooled>();
    }

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
        if (gameObject.activeInHierarchy)
        {
            transform.position += transform.right * Time.deltaTime * speed;
        }
    }

    private void OnBecameInvisible()
    {
        // if (!gameObject.activeInHierarchy)
        // {
        //     return;
        // }

        // StartCoroutine(Unload());
    }

    private IEnumerator Unload()
    {
        yield return Helpers.GetWait(0.5f);
        objectPooled.Release();
    }

    // private void OnEnable() {
    //     StopCoroutine(autoDestroyCoroutine);
    //     autoDestroyCoroutine = StartCoroutine(AutoDestroy(0.35f));
    // }

    private void OnDisable()
    {
        if (objectPooled.Pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            objectPooled.Release();
        }
    }

    IEnumerator AutoDestroy(float duration = 0.5f)
    {
        yield return new WaitForSeconds(duration);

        if (objectPooled.Pool == null)
        {
            Destroy(gameObject);
        }
        else
        {
            objectPooled.Release();
        }
    }

}
