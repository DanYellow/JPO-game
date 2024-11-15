using UnityEngine;

public class WaveEffectCollision : MonoBehaviour
{
    [SerializeField, Range(2, 10)]
    private float speed = 7;

    private ObjectPooled objectPooled;

    private Vector3 originPosition;

    [SerializeField]
    private VoidEventChannel onGameEndEvent;

    private void Awake()
    {
        objectPooled = GetComponent<ObjectPooled>();
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        onGameEndEvent.OnEventRaised -= OnGameEnd;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            Vector3 dirX = (other.transform.position - transform.position).normalized;
            playerHealth.TakeDamage(dirX);
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
        onGameEndEvent.OnEventRaised += OnGameEnd;
        originPosition = gameObject.transform.position;
    }
}
