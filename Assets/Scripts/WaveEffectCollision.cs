using UnityEngine;

public class WaveEffectCollision : MonoBehaviour
{
    private ObjectPooled objectPooled;

    [Header("Scriptable Objects"), SerializeField]
    private VoidEventChannel onGameEndEvent;
    [SerializeField]
    private VoidEventChannel onTimerEndEvent;

    public GameObject vfx;

    private void Awake()
    {
        objectPooled = GetComponent<ObjectPooled>();
    }

    private void OnGameEnd()
    {
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        onGameEndEvent.OnEventRaised += OnGameEnd;
        onTimerEndEvent.OnEventRaised += OnGameEnd;
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
        if (!vfx.activeInHierarchy)
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

    private void OnDisable()
    {
        onGameEndEvent.OnEventRaised -= OnGameEnd;
        onTimerEndEvent.OnEventRaised -= OnGameEnd;
    }
}
