using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolingGenerator : MonoBehaviour
{

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;

    private ObjectPooling objectPooling;

    [SerializeField]
    private float delayBetweenGeneration = 0.75f;
    private float timer = 0f;
    public float delayBetweenDelayUpdate = 7;


    private void Awake()
    {
        objectPooling = GetComponent<ObjectPooling>();
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
        GameObject objectPooled = objectPooling.CreateObject("obstacle");

        if (objectPooled != null)
        {
            Obstacle bullet = objectPooled.GetComponent<Obstacle>();
            bullet.Initialize();
        }
    }

    IEnumerator Generate()
    {
        List<ObjectPoolItem> listItemsToPool = objectPooling.listItemsToPool.Where(item => item.extInit == false).ToList();

        foreach (ObjectPoolItem obj in listItemsToPool)
        {
            for (var i = 0; i < obj.poolSize; i++)
            {
                GameObject objectPooled = objectPooling.CreateObject(obj.key);
                if (objectPooled != null)
                {
                    Obstacle bullet = objectPooled.GetComponent<Obstacle>();
                    bullet.Initialize();
                    yield return new WaitForSeconds(Random.Range(0.15f, 0.75f));
                }
            }
        }
        StartCoroutine(CreateObstacle());
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= delayBetweenDelayUpdate)
        {
            timer = 0f;
            delayBetweenGeneration = Mathf.Clamp(delayBetweenGeneration - 0.05f, 0.15f, 0.75f);
        }
    }

    IEnumerator CreateObstacle()
    {
        while (true)
        {
            Create();
            yield return new WaitForSeconds(delayBetweenGeneration);
        }
    }

    private void OnDestroy()
    {
        onPlayerDeathVoidEventChannel.OnEventRaised -= StopGeneration;
    }
}
