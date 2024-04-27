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

    [SerializeField]
    private Transform spawnPoint;

    private ObjectPooling meteorPooling;

    private void Awake()
    {
        planet = GetComponent<GravityAttractor>();
        meteorPooling = GetComponent<ObjectPooling>();
    }

    IEnumerator Start()
    {
        yield break;
        yield return Helpers.GetWait(3f);
        // StartCoroutine(SpawnMeteor());
        while (true)
        {
            if (Spawn() == null)
            {
                yield return null;
                
            }
            yield return Helpers.GetWait(spawnTime);
        }
        // yield return StartCoroutine(SpawnMeteor());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
        Vector3 groundNormal = spawnPoint.position - transform.position;

        Vector3 forwardsVector = -Vector3.Cross(groundNormal, transform.right);
        // Finally, compose the two directions back into a single rotation.
        spawnPoint.rotation = Quaternion.LookRotation(forwardsVector, groundNormal);
        // target.parent.LookAt(transform, -Vector3.forward);
    }

    private ObjectPooled Spawn()
    {
        ObjectPooled objectPooled = meteorPooling.Get("meteor");

        if (objectPooled != null)
        {
            objectPooled.gameObject.SetActive(false);
            Vector3 pos = new Vector3(
                spawnPoint.position.x + (Random.insideUnitSphere.x * distance),
                spawnPoint.position.y,
                spawnPoint.position.z + (Random.insideUnitSphere.z * distance)
            );

            objectPooled.transform.SetPositionAndRotation(pos, Quaternion.identity);
            MeteorContainer meteorContainer = objectPooled.GetComponent<MeteorContainer>();
            meteorContainer.ResetThyself();

            Meteor meteor = objectPooled.GetComponentInChildren<Meteor>();
            meteor.hitTarget = transform;
            meteor.transform.LookAt(transform);
            // print(objectPooled.name + " " + gravityBody.transform.position + " " + gravityBody.transform.localPosition);

            objectPooled.gameObject.SetActive(true);
        }

        return objectPooled;
    }


    // IEnumerator SpawnMeteor()
    // {
    //     Vector3 pos = new Vector3(
    //         target.position.x + (Random.insideUnitCircle.x * distance),
    //         target.position.y + (Random.insideUnitCircle.y * distance),
    //         target.position.z
    //     );
    //     GameObject meteor = Instantiate(meteorPrefab, Vector3.zero, Quaternion.identity);
    //     meteor.transform.LookAt(gameObject.transform);
    //     meteor.GetComponentInChildren<GravityBody>().planet = planet;

    //     yield return Helpers.GetWait(spawnTime);

    //     StartCoroutine(SpawnMeteor());
    // }
}
