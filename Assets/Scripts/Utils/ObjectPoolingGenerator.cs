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
    [Tooltip("Define how often the pool interval can be updated. -1 if not needed")]
    public float whenDelayBetweenNewItemPooledUpdated = -1;
    public float stepDecreaseNewItem = 0.01f;
    private float timerBetweenNewItemPooledUpdate = 0f;

    private void Awake()
    {
        objectPooling = FindObjectOfType<ObjectPoolingManager>(false);
        onPlayerDeathVoidEventChannel.OnEventRaised += StopPooling;
    }

    public void StartGame()
    {
        StartCoroutine(Generate());
    }

    private void StopPooling()
    {
        StopAllCoroutines();
    }

    IEnumerator Generate()
    {
        // Create pool of objects 0.0005
        ObjectPoolItemData obj = objectPooling.listItemsToPool.First((item) => item.key == key);
        int initialPoolSize = Mathf.CeilToInt(obj.poolSize * obj.ratioInitialPoolSizeSpawned);

        for (var i = 0; i < initialPoolSize; i++)
        {
            objectPooling.CreateObject(key);
            yield return new WaitForSeconds(Random.Range(0.15f, 0.55f));
        }

        StartCoroutine(Create());
        StartCoroutine(DecreaseNewPoolTime());
    }

    IEnumerator DecreaseNewPoolTime()
    {
        while (true)
        {
            timerBetweenNewItemPooledUpdate += Time.deltaTime;

            if (whenDelayBetweenNewItemPooledUpdated > 0 && timerBetweenNewItemPooledUpdate >= whenDelayBetweenNewItemPooledUpdated)
            {
                timerBetweenNewItemPooledUpdate = 0f;
                delayBetweenNewItemPooled = Mathf.Clamp(
                    delayBetweenNewItemPooled - stepDecreaseNewItem,
                    stepDecreaseNewItem,
                    0.75f
                );
                delayBetweenNewItemPooled = float.Parse(delayBetweenNewItemPooled.ToString("0.000"));
            }

            yield return null;
        }
    }

    IEnumerator Create()
    {
        // WaitForSeconds intervalNewItemPooled = new WaitForSeconds(0);
        WaitForSeconds intervalNewItemPooled = new WaitForSeconds(delayBetweenNewItemPooled);

        while (true)
        {
            yield return intervalNewItemPooled;
            objectPooling.CreateObject(key);
        }
    }

    private void OnDestroy()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= StopPooling;
    }
}
