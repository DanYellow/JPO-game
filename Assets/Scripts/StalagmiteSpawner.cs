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
        yield return new WaitUntil(() => hasReachMinimumTravelDistance.CurrentValue == true);
        while (true)
        {
            yield return StartCoroutine(Spawn());
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
    }

    private IEnumerator Spawn()
    {
        float sphereColliderScale = sphereCollider.transform.lossyScale.x;
        Vector3 randPosition = Random.onUnitSphere;
        Vector3 endPosition = randPosition * sphereCollider.radius * sphereColliderScale;
        stalagmiteMarkerGO.transform.position = endPosition;
        stalagmiteMarkerGO.SetActive(true);

        yield return stalagmiteMarker.Appear();

        GameObject stalagmiteGO = Instantiate(stalagmitePrefab, Vector3.zero, Quaternion.identity);
        Stalagmite stalagmite = stalagmiteGO.GetComponent<Stalagmite>();
        BoxCollider boxCollider = stalagmiteGO.GetComponent<BoxCollider>();
        stalagmite.target = transform;
        stalagmite.endPosition = randPosition * (sphereCollider.radius + (boxCollider.bounds.size.y * 2 / sphereColliderScale)) * sphereColliderScale;
    }

    private void OnDisable()
    {
        onGameOver.OnEventRaised -= GameOver;
    }
}
