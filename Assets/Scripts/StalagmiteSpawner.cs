using System.Collections;
using UnityEngine;

public class StalagmiteSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject stalagmitePrefab;

    [SerializeField]
    private float spawnTime = 5f;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue hasReachMinimumTravelDistanceForStalagmites;
    
    IEnumerator Start()
    {
        yield return new WaitUntil(() => hasReachMinimumTravelDistanceForStalagmites.CurrentValue == true);
        while (true)
        {
            Spawn();
            yield return Helpers.GetWait(spawnTime);
        }
    }

    private void Spawn() {
        GameObject stalagmite = Instantiate(stalagmitePrefab, Vector3.zero, Quaternion.identity);
        stalagmite.GetComponent<Stalagmite>().target = transform;
    }
}
