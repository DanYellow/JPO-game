using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float distance = 20f;

    private GravityAttractor planet;

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
        Vector3 pos = Random.onUnitSphere * 100f;
        GameObject meteor = Instantiate(meteorPrefab, pos, Quaternion.identity);
        meteor.GetComponent<GravityBody>().planet = planet;

        yield return new WaitForSeconds(1f);

        StartCoroutine(SpawnMeteor());
    }
}
