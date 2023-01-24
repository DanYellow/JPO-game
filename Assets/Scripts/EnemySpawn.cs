using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyToSpawn;

    [SerializeField]
    private VoidEventChannel onEvent;

    private UnityAction onEventProxy;

    private void Awake()
    {
        onEventProxy = () => { StartCoroutine(SpawnEnemy()); };
        onEvent.OnEventRaised += onEventProxy;
        transform.position = enemyToSpawn.transform.position;
    }

    IEnumerator SpawnEnemy()
    {
        yield return new WaitForSeconds(0.75f);
        Instantiate(enemyToSpawn, transform.position, enemyToSpawn.transform.rotation);
    }
    
    private void OnDestroy()
    {
        onEvent.OnEventRaised -= onEventProxy;
    }
}
