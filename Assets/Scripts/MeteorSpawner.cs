using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject meteorPrefab;

    [SerializeField]
    private float distance = 20f;

    [SerializeField]
    private float spawnTime = 7f;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private Transform spawnPoint;

    private ObjectPooling meteorPooling;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    [SerializeField]
    private CarData carData;

    [SerializeField]
    private BoolValue isCarDrifting;

    private void Awake()
    {
        meteorPooling = GetComponent<ObjectPooling>();
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => hasReachMinimumTravelDistance.CurrentValue == true);
        // StartCoroutine(SpawnMeteor());
        while (true)
        {
            if (carData.currentVelocity < 5 || Spawn() == null)
            {
                yield return null;

            }
            yield return Helpers.GetWait(spawnTime);
        }
        // yield return StartCoroutine(SpawnMeteor());
    }

    private void Update()
    {
        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
        #endif
        Vector3 groundNormal = spawnPoint.position - transform.position;

        Vector3 forwardsVector = -Vector3.Cross(groundNormal, transform.right);
        // Finally, compose the two directions back into a single rotation.
        // spawnPoint.rotation = Quaternion.LookRotation(forwardsVector, groundNormal);
        // target.parent.LookAt(transform, -Vector3.forward);

        // Vector3 dir = (target.position - spawnPoint.position).normalized;
        // var rotation = Quaternion.LookRotation(dir);
        // spawnPoint.localRotation = Quaternion.Lerp(spawnPoint.rotation, rotation, 1);

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
            meteor.hitTarget = isCarDrifting.CurrentValue ? target : transform;
            meteor.speedFactor = carData.isMovingBackward ? 5.5f : 1;
            meteor.transform.LookAt(transform);

            objectPooled.gameObject.SetActive(true);
        }

        return objectPooled;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(spawnPoint.position, distance);
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
