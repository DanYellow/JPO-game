using System.Collections;
using UnityEngine;
using System.Linq;

public class ObjectPoolingGenerator : MonoBehaviour
{
    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    [HideInInspector]
    private ObjectPoolingManager objectPooling;

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
        objectPooling = FindObjectOfType<ObjectPoolingManager>();

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

    IEnumerator Generate()
    {
        ObjectPoolItemData obj = objectPooling.listItemsToPool.First((item) => item.key == key);

        for (var i = 0; i < obj.poolSize; i++)
        {
            objectPooling.CreateObject(key);
        }
        yield return new WaitForSeconds(Random.Range(0.15f, 0.75f));

        StartCoroutine(Create());
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
        onPlayerDeathVoidEventChannel.OnEventRaised -= StopGeneration;
    }
}
