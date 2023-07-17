using System.Collections;
using UnityEngine;
using System.Linq;

public class ObjectPoolingGenerator : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    private ObjectPoolingManager objectPooling;

    [SerializeField]
    private string key = "";

    [Header("Pooling")]
    [SerializeField, Tooltip("Interval between creation / new pool of an object")]
    private float delayBetweenNewItemPooled = 0.75f;
    [Tooltip("Define how often the pool interval can be update. -1 if not needed")]
    public float whenDelayBetweenNewItemPooledUpdated = -1;
    private float timerBetweenNewItemPooledUpdate = 0f;

    [Header("Upgrade pool size")]
    [SerializeField]
    private float delayBetweenPoolSizeUpdate = 15f;
    private float timerPoolSizeUpdate = 0f;

    [SerializeField]
    private int nbSlotsAddable = 10;

    [SerializeField]
    private VoidEventChannel onStartPlayEvent;

    private void Awake()
    {
        objectPooling = FindObjectOfType<ObjectPoolingManager>(false);
        onPlayerDeathVoidEventChannel.OnEventRaised += StopPooling;
        // onStartPlayEvent.OnEventRaised += OnStart;
    }

    private void Start()
    {
        if(onStartPlayEvent == null) {
            StartCoroutine(Generate());
        }
        // StartCoroutine(Create());
    }

    public void StartGame() {
        StartCoroutine(Generate());
    }

    private void StopPooling()
    {
        StopAllCoroutines();
    }

    IEnumerator Generate()
    {
        // Create pool of objects
        ObjectPoolItemData obj = objectPooling.listItemsToPool.First((item) => item.key == key);
        for (var i = 0; i < obj.poolSize; i++)
        {
            objectPooling.CreateObject(key);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.65f));
        }

        StartCoroutine(Create());
    }

    private void Update()
    {
        timerBetweenNewItemPooledUpdate += Time.deltaTime;
        timerPoolSizeUpdate += Time.deltaTime;

        if (whenDelayBetweenNewItemPooledUpdated > 0 && timerBetweenNewItemPooledUpdate >= whenDelayBetweenNewItemPooledUpdated)
        {
            timerBetweenNewItemPooledUpdate = 0f;
            delayBetweenNewItemPooled = Mathf.Clamp(delayBetweenNewItemPooled - 0.05f, 0.05f, 0.75f);
            delayBetweenNewItemPooled = float.Parse(delayBetweenNewItemPooled.ToString("0.00"));
        }

        if (
            delayBetweenPoolSizeUpdate > 0 &&
            timerPoolSizeUpdate >= delayBetweenPoolSizeUpdate
        )
        {
            timerPoolSizeUpdate = 0f;
            if (objectPooling.listDictItemsToPool.TryGetValue(key, out ObjectPoolItemData itemToPool))
            {
                itemToPool.poolSize += nbSlotsAddable;
            }
        }
    }

    IEnumerator Create()
    {
        while (true)
        {
            objectPooling.CreateObject(key);
            yield return new WaitForSeconds(delayBetweenNewItemPooled);
        }
    }

    private void OnDestroy()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= StopPooling;
        // onStartPlayEvent.OnEventRaised -= OnStart;
    }
}
