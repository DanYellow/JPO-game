using System.Collections;
using UnityEngine;

public class ObjectPoolingGenerator : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    [HideInInspector]
    private ObjectPooling objectPooling;

    [SerializeField]
    private float delayBetweenNewItemPooled = 0.75f;
    private float timerBetweenNewItemPooledUpdate = 0f;
    public float delayBetweenNewItemPooledUpdate = 7;
    public bool canUpdateDelayBetweenNewItemPooled = false;

    private float timerPoolSizeUpdate = 0f;
    private float delayBetweenPoolSizeUpdate = 15f;

    [SerializeField]
    private int nbSlotsAddable = 10;

    [SerializeField]
    private string key = "";

    private void Awake()
    {
        objectPooling = FindObjectOfType<ObjectPooling>();

        if (!canUpdateDelayBetweenNewItemPooled)
        {
            delayBetweenNewItemPooledUpdate = -1;
        }
    }

    private void Start()
    {
        StartCoroutine(Generate());
        onPlayerDeathVoidEventChannel.OnEventRaised += StopGeneration;
    }

    private void StopGeneration()
    {
        StopAllCoroutines();
    }

    private void Create()
    {
        GameObject objectPooled = objectPooling.CreateObject(key);
    }

    IEnumerator Generate()
    {
        foreach (ObjectPoolItemData obj in objectPooling.listItemsToPool)
        {
            for (var i = 0; i < obj.poolSize; i++)
            {
                GameObject objectPooled = objectPooling.CreateObject(obj.key);
                yield return new WaitForSeconds(Random.Range(0.15f, 0.75f));
            }
        }
        StartCoroutine(CreateObstacle());
    }

    private void Update()
    {
        timerBetweenNewItemPooledUpdate += Time.deltaTime;
        timerPoolSizeUpdate += Time.deltaTime;

        if (timerBetweenNewItemPooledUpdate >= delayBetweenNewItemPooledUpdate)
        {
            timerBetweenNewItemPooledUpdate = 0f;
            if (canUpdateDelayBetweenNewItemPooled)
            {
                delayBetweenNewItemPooled = Mathf.Clamp(delayBetweenNewItemPooled - 0.05f, 0.15f, 0.75f);
            }
        }

        if (timerPoolSizeUpdate >= delayBetweenPoolSizeUpdate)
        {
            timerPoolSizeUpdate = 0f;
            if (objectPooling.listDictItemsToPool.TryGetValue(key, out ObjectPoolItemData itemToPool))
            {
                itemToPool.poolSize += nbSlotsAddable;
            }
        }
    }

    IEnumerator CreateObstacle()
    {
        while (true)
        {
            Create();
            yield return new WaitForSeconds(delayBetweenNewItemPooled);
        }
    }

    private void OnDestroy()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= StopGeneration;
    }
}
