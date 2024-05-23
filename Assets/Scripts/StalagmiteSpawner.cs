using System.Collections;
using UnityEngine;

public class StalagmiteSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject stalagmitePrefab;

    [SerializeField]
    private float spawnTime = 5f;

    private SphereCollider sphereCollider;

    [SerializeField]
    private GameObject stalagmiteMarkerGO;

    private StalagmiteMarker stalagmiteMarker;

    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;
    [SerializeField]
    private VoidEventChannel onGameOver;

    public Transform target;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        stalagmiteMarker = stalagmiteMarkerGO.GetComponent<StalagmiteMarker>();
    }

    private void OnEnable()
    {
        onGameOver.OnEventRaised += GameOver;
    }

    private void GameOver()
    {
        StopAllCoroutines();
    }

    IEnumerator Start()
    {
        // Vector3 targetDir = new Vector3(0, 45.38f, 0) - transform.position;
        // float angle = Vector3.Angle(targetDir, transform.forward);

        // for (int i = 0; i < 10; i++)
        // {
        //     var randomPosition = GetRandomPosition(35, sphereCollider.radius * sphereCollider.transform.lossyScale.x);
        //     Debug.DrawLine(transform.position, randomPosition, Color.green, 100f);
        // }

        yield return new WaitUntil(() => hasReachMinimumTravelDistance.CurrentValue == true);
        while (true)
        {
            yield return StartCoroutine(Spawn());
            yield return Helpers.GetWait(spawnTime);
        }
    }

    // https://stackoverflow.com/questions/64623448/random-rotation-on-a-3d-sphere-given-an-angle
    private Vector3 GetRandomPosition(float angle, float radius)
    {
        var rotationX = Quaternion.AngleAxis(Random.Range(-angle, angle), target.right);
        var rotationZ = Quaternion.AngleAxis(Random.Range(-angle, angle), target.forward);
        var position = rotationZ * rotationX * target.up * radius;

        return position;
    }

    // private Vector3 GetRandomPosition(float angle, float radius)
    // {
    //     var rotationX = Quaternion.AngleAxis(Random.Range(-angle, angle), transform.right);
    //     var rotationZ = Quaternion.AngleAxis(Random.Range(-angle, angle), transform.forward);
    //     var position = rotationZ * rotationX * transform.up * radius + transform.position;

    //     return position;
    // }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
           StartCoroutine( Spawn());
        // print( 
        //     2 * Mathf.PI * Mathf.Pow(sphereCollider.radius * sphereCollider.transform.lossyScale.x, 2) * (1 - Mathf.Cos(20))
        // );
    //    print(2πr2(1-cosθ));
        }
#endif
    }

    private IEnumerator Spawn()
    {
        float sphereColliderScale = sphereCollider.transform.lossyScale.x;
        Vector3 randPosition = Random.onUnitSphere;
        var endPosition = GetRandomPosition(35, sphereCollider.radius * sphereCollider.transform.lossyScale.x);

        // Vector3 endPosition = randPosition * sphereCollider.radius * sphereColliderScale;
        stalagmiteMarkerGO.transform.position = endPosition;
        stalagmiteMarkerGO.SetActive(true);

        yield return stalagmiteMarker.Appear();

        GameObject stalagmiteGO = Instantiate(stalagmitePrefab, Vector3.zero, Quaternion.identity);
        stalagmiteGO.transform.parent = transform;
        Stalagmite stalagmite = stalagmiteGO.GetComponent<Stalagmite>();
        BoxCollider boxCollider = stalagmiteGO.GetComponent<BoxCollider>();
        stalagmite.target = transform;
        stalagmite.endPosition = endPosition + (boxCollider.bounds.size.y * Vector3.up); //randPosition * (sphereCollider.radius + (boxCollider.bounds.size.y * 2 / sphereColliderScale)) * sphereColliderScale;
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= GameOver;
    }
}
