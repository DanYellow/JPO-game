using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[System.Serializable]
public class ObjectPoolItemData
{
    [SerializeField, Tooltip("Sets the limit of objects created in memory (expandable dynamically)")]
    public int poolSize = 5;
    public string key = "";

    [Tooltip("Defines the prefab to instanciate / pool")]
    public GameObject prefab;
    public Queue<GameObject> queueObjectsPooled = new Queue<GameObject>();

    public bool extInit = false;
    public bool isExtendable = false;
}

// More info : 
// https://www.youtube.com/watch?v=YCHJwnmUGDk
// https://gameprogrammingpatterns.com/object-pool.html
public class ObjectPooling : MonoBehaviour
{
    public List<ObjectPoolItemData> listItemsToPool = new List<ObjectPoolItemData>();

    public Dictionary<string, ObjectPoolItemData> listDictItemsToPool = new Dictionary<string, ObjectPoolItemData>();

    private void Start()
    {
        foreach (ObjectPoolItemData obj in listItemsToPool)
        {
            string key = (obj.key != "") ? obj.key : obj.prefab.name;
            listDictItemsToPool.Add(key, obj);
        }
    }

    public GameObject CreateObject(string key = "")
    {
        GameObject poolObject = null;

        if (listDictItemsToPool.TryGetValue(key, out ObjectPoolItemData itemToPool))
        {
            Queue<GameObject> queueObjectsPooled = itemToPool.queueObjectsPooled;

            int nbItemsActive = queueObjectsPooled.ToList().Count(obj => obj.activeSelf);
            // bool allObjectsActive = queueObjectsPooled.ToList().All(obj => obj.activeSelf);

            if (nbItemsActive == itemToPool.poolSize) {
                return poolObject;
            };
            if (queueObjectsPooled.Count < itemToPool.poolSize)
            {
                poolObject = Instantiate(itemToPool.prefab, transform.position, Quaternion.identity);
                poolObject.name = $"{transform.name}_{itemToPool.prefab.name}_{queueObjectsPooled.Count}";
            }
            else
            {
                poolObject = queueObjectsPooled.Dequeue();
                poolObject.transform.position = transform.position;
                poolObject.transform.rotation = Quaternion.identity;

                poolObject.SetActive(true);
            }

            queueObjectsPooled.Enqueue(poolObject);
        }

        return poolObject;
    }

    private void OnDestroy()
    {
        foreach (ObjectPoolItemData itemToPool in listItemsToPool)
        {
            Queue<GameObject> queueObjectsPooled = itemToPool.queueObjectsPooled;

            foreach (GameObject obj in queueObjectsPooled.ToList().Where(poolObj =>
            {
                return !poolObj.activeSelf || poolObj != null;
            }))
            {
                if (obj != null)
                {
                    Destroy(obj);
                }
            }
        }
    }
}
