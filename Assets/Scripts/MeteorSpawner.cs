using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

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

    public IObjectPool<MeteorContainer> pool;

    private void Awake()
    {
        planet = GetComponent<GravityAttractor>();
        meteorPooling = GetComponent<ObjectPooling>();
    }

    IEnumerator Start()
    {
        yield return Helpers.GetWait(0.5f);
        // StartCoroutine(SpawnMeteor());
        while (true)
        {
            ObjectPooled objectPooled = meteorPooling.Get("meteor");

            if (objectPooled != null)
            {
                Vector3 pos = new Vector3(
                    target.position.x + (Random.insideUnitSphere.x * distance),
                    target.position.y,
                    target.position.z + (Random.insideUnitSphere.z * distance)
                );
                objectPooled.transform.position = pos;
                // objectPooled.transform.SetPositionAndRotation(
                //     pos,
                //     Quaternion.identity
                // );
                // objectPooled.transform.LookAt(gameObject.transform); (3.69, 71.17, 6.49)
                objectPooled.GetComponentInChildren<GravityBody>().planet = planet;
                MeteorContainer meteorContainer = objectPooled.GetComponent<MeteorContainer>();
                meteorContainer.ResetThyself();

                yield return Helpers.GetWait(5);
            }
            yield return null;
        }

        yield return StartCoroutine(SpawnMeteor());
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
