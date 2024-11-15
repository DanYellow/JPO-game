using UnityEngine;

public class WaveEffectCollision : MonoBehaviour
{
    [SerializeField, Range(2, 10)]
    private float speed = 7;

    private ObjectPooled objectPooled;

    private Vector3 originPosition;

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
        Unload();
    }

    private void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.position += transform.right * Time.deltaTime * speed;
        }

        if (Vector3.Distance(gameObject.transform.position, originPosition) > 25 && gameObject.activeInHierarchy)
        {
            Unload();
        }
    }

    private void Unload()
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

    private void OnEnable()
    {
        originPosition = gameObject.transform.position;
    }
}
