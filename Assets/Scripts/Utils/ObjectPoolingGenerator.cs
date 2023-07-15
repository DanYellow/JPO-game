using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ObjectPoolingGenerator : MonoBehaviour
{

    private ObjectPooling objectPooling;

    private void Awake()
    {
        objectPooling = GetComponent<ObjectPooling>();
    }

    private void Start()
    {
        StartCoroutine(Generate());
        // InvokeRepeating(nameof(Create), 0.5f, 2f);
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
        InvokeRepeating(nameof(Create), 0, 0.15f);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
