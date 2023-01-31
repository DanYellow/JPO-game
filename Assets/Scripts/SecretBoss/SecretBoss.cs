using System.Collections;
using UnityEngine;

public class SecretBoss : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private Transform laserFirePoint;

    [SerializeField]
    private GameObject laserPrefab;

    [Header("Parts")]
    [SerializeField]
    public GameObject torso;
    [SerializeField]
    private GameObject frontArm;
    [SerializeField]
    private GameObject backArm;

    private GameObject laser;

    private Vector2 initTorsoPosition;

    [SerializeField]
    VoidEventChannel OnTorsoDeathChannel;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        OnTorsoDeathChannel.OnEventRaised += DestroyParts;

        initTorsoPosition = torso.transform.localPosition;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            frontArm.transform.position += new Vector3(20, 0, 0);
        }
    }

    private void DestroyParts()
    {
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            Destroy(g.gameObject);
        }
    }

    public void MoveToTarget(Vector2 targetPos, Bounds size) {
        float lerpDuration = 1.5f; 

        bool isTargetLowerThanTorso = transform.InverseTransformPoint(targetPos).y < initTorsoPosition.y;
        Vector2 endValue = new Vector2(
            torso.transform.localPosition.x,
            isTargetLowerThanTorso ? transform.InverseTransformPoint(size.max).y : transform.InverseTransformPoint(size.min).y
        );

        if(endValue.y > initTorsoPosition.y) {
            endValue.y = initTorsoPosition.y;
        }

        StartCoroutine(MoveTorsoTo(endValue, lerpDuration));
        StartCoroutine(ShootLaser());
    }

    IEnumerator MoveTorsoTo(Vector2 endPosition, float duration) {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            torso.transform.localPosition = Vector2.Lerp(
                torso.transform.localPosition, 
                endPosition, 
                timeElapsed / duration
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        torso.transform.localPosition = endPosition;
    }

    IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(0.25f);
        laser.transform.position = laserFirePoint.position;
        laser.SetActive(true);
        yield return new WaitForSeconds(0.45f);
        laser.SetActive(false);

        yield return new WaitForSeconds(0.90f);
        StartCoroutine(MoveTorsoTo(initTorsoPosition, 2f));
    }

    private void OnDisable()
    {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
    }
}
