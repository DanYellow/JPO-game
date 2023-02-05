using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyToSpawn;

    [SerializeField]
    private VoidEventChannel onEvent;

    [SerializeField]
    private float delayBetweenSpawn = 0.75f;

    public int numberOfSpawn = int.MaxValue;

    private UnityAction onEventProxy;

    private void Awake()
    {
        onEventProxy = () => { StartCoroutine(SpawnEnemy()); };
        onEvent.OnEventRaised += onEventProxy;
        transform.position = enemyToSpawn.transform.position;
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(delayBetweenSpawn);
        Instantiate(enemyToSpawn, transform.position, enemyToSpawn.transform.rotation);
        numberOfSpawn--;
        if(numberOfSpawn == 0) {
            Destroy(gameObject);
        }
    }
    
    private void OnDestroy()
    {
        onEvent.OnEventRaised -= onEventProxy;
    }
}
