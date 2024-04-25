using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject meteorPrefab;

    [SerializeField]
    private float distance = 20f;

    private GravityAttractor planet;

    [SerializeField]
    private float spawnTime = 7f;

    [SerializeField]
    private Transform target;

    private void Awake()
    {
        planet = GetComponent<GravityAttractor>();
    }

    void Start()
    {
        StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnMeteor()
    {
        Vector3 pos = new Vector3(
            target.position.x + (Random.onUnitSphere.x * distance),
            target.position.y + (Random.onUnitSphere.y * distance),
            target.position.z
        );
        GameObject meteor = Instantiate(meteorPrefab, pos, Quaternion.identity);
        meteor.transform.LookAt(gameObject.transform);
        meteor.GetComponentInChildren<GravityBody>().planet = planet;

        yield return Helpers.GetWait(spawnTime);

        StartCoroutine(SpawnMeteor());
    }
}
