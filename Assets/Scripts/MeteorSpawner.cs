using System.Collections;
using UnityEngine;

public class MeteorSpawner : MonoBehaviour
{
    public GameObject meteorPrefab;
    public float distance = 20f;

    private GravityAttractor planet;

    [SerializeField]
    private float spawnTime = 7f;

    [SerializeField]
    private Transform target;

    public GameObject prefab;

    private void Awake()
    {
        planet = GetComponent<GravityAttractor>();
    }

    void Start()
    {
        StartCoroutine(SpawnMeteor());
    }

    private void Update()
    {
        // Vector3 gravityUp = (target.position - transform.position).normalized;
        // print(gravityUp);
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     GameObject go = Instantiate(
        //          prefab,
        //          Vector3.zero,
        //          //  new Vector3(target.position.x, target.position.y, 50 * gravityUp.normalized.y - target.position.z),
        //          Quaternion.identity
        //     );
        //     go.transform.position = new Vector3(
        //         target.position.x,
        //         target.position.y,
        //         target.position.z
        //     );
        //     go.transform.LookAt(gameObject.transform);
        //     // go.transform.rotation = target.rotation;
        // }
    }

    IEnumerator SpawnMeteor()
    {
        Vector3 pos = new Vector3(
            target.position.x + (Random.onUnitSphere.x * 20f),
            target.position.y + (Random.onUnitSphere.y * 20f),
            target.position.z
        );
        GameObject meteor = Instantiate(meteorPrefab, pos, Quaternion.identity);
        meteor.transform.LookAt(gameObject.transform);
        meteor.GetComponent<GravityBody>().planet = planet;

        yield return Helpers.GetWait(1);

        StartCoroutine(SpawnMeteor());
    }
}
