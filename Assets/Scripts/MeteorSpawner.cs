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

    private ObjectPooling meteorPooling;

    private void Awake()
    {
        planet = GetComponent<GravityAttractor>();
        meteorPooling = GetComponent<ObjectPooling>();
    }

    IEnumerator Start()
    {
        // yield return Helpers.GetWait(0.5f);
        // StartCoroutine(SpawnMeteor());
        while (true)
        {
            ObjectPooled objectPooled = meteorPooling.Get("meteor");

            if (objectPooled != null)
            {
                objectPooled.gameObject.SetActive(false);
                Vector3 pos = new Vector3(
                    target.position.x + (Random.insideUnitSphere.x * distance),
                    target.position.y,
                    target.position.z + (Random.insideUnitSphere.z * distance)
                );
                
                objectPooled.transform.SetPositionAndRotation(pos, Quaternion.identity);
                MeteorContainer meteorContainer = objectPooled.GetComponent<MeteorContainer>();
                meteorContainer.ResetThyself();

                GravityBody gravityBody = objectPooled.GetComponentInChildren<GravityBody>();
                gravityBody.planet = planet;
                print(objectPooled.name + " " + gravityBody.transform.position + " " + gravityBody.transform.localPosition);
                // gravityBody.transform.LookAt(transform);

                objectPooled.gameObject.SetActive(true);
                yield return Helpers.GetWait(5);
            }
            yield return null;
        }

        // yield return StartCoroutine(SpawnMeteor());
    }

    IEnumerator SpawnMeteor()
    {
        Vector3 pos = new Vector3(
            target.position.x + (Random.insideUnitCircle.x * distance),
            target.position.y + (Random.insideUnitCircle.y * distance),
            target.position.z
        );
        GameObject meteor = Instantiate(meteorPrefab, Vector3.zero, Quaternion.identity);
        meteor.transform.LookAt(gameObject.transform);
        meteor.GetComponentInChildren<GravityBody>().planet = planet;

        yield return Helpers.GetWait(spawnTime);

        StartCoroutine(SpawnMeteor());
    }
}
