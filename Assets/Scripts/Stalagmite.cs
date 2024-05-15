using System.Collections;
using UnityEngine;

public class Stalagmite : MonoBehaviour
{
    public Transform target;

    public Vector3 endPosition;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;
    [SerializeField]
    private VoidEventChannel onCameraSwitch;

    void Start()
    {
        transform.position = Vector3.zero;

        transform.up = target.up;

        StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        float current = 0;
        float duration = 0.65f;
        Vector3 startPosition = transform.position;

        while (current <= 1)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, current);
            current += Time.deltaTime / duration;
            if (transform.position != Vector3.zero)
            {
                transform.LookAt(target);
                // transform.rotation = Quaternion.LookRotation(target.position - transform.position);
            }

            yield return null;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            onGameOver.Raise();
            onCameraSwitch.Raise();
        }
    }
}
