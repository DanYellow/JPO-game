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

    private bool isSpawnPointBehindTarget = false;

    private ObjectPooling meteorPooling;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    [SerializeField]
    private CarData carData;

    [SerializeField]
    private BoolValue isCarDrifting;

    [SerializeField]
    private VoidEventChannel onGameOver;

    private void Awake()
    {
        meteorPooling = GetComponent<ObjectPooling>();
    }

    private void OnEnable()
    {
        onGameOver.OnEventRaised += GameOver;
    }

    IEnumerator Start()
    {
        yield return new WaitUntil(() => hasReachMinimumTravelDistance.CurrentValue == true);
        while (true)
        {
            if (carData.currentVelocity < 5 || Spawn() == null)
            {
                yield return null;
            }
            yield return Helpers.GetWait(spawnTime);
        }
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
#endif
        if (
            (!isSpawnPointBehindTarget && carData.isMovingBackward) ||
            (isSpawnPointBehindTarget && !carData.isMovingBackward)
        )
        {
            isSpawnPointBehindTarget = !isSpawnPointBehindTarget;
            spawnPoint.parent.RotateAround(target.position, target.up, 180);
        }
    }

    private void GameOver()
    {
        StopAllCoroutines();
    }

    private ObjectPooled Spawn()
    {
        ObjectPooled objectPooled = meteorPooling.Get("meteor");

        if (objectPooled != null)
        {
            objectPooled.gameObject.SetActive(false);

            float finalRadius = distance * (isSpawnPointBehindTarget ? 0.25f : 1);

            Vector3 pos = new Vector3(
                spawnPoint.position.x + (Random.insideUnitSphere.x * finalRadius),
                spawnPoint.position.y,
                spawnPoint.position.z + (Random.insideUnitSphere.z * finalRadius)
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

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= GameOver;
    }
}
