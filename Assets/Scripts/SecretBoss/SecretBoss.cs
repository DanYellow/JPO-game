using System.Collections;
using UnityEngine;

public class SecretBoss : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    private Transform laserFirePoint;

    [SerializeField]
    private GameObject laserPrefab;
    private GameObject laser;

    [Header("Parts")]
    [SerializeField]
    public GameObject torso;
    [SerializeField]
    private GameObject frontArm;
    [SerializeField]
    private GameObject backArm;


    private Vector2 initTorsoPosition;
    private Vector2 initFrontArmPosition;
    private Vector2 initBackArmPosition;

    [SerializeField]
    VoidEventChannel OnTorsoDeathChannel;

    // Time delays
    private float timeDelayBeforeShot = 0.5f;
    private float timeDelayBeforeLaserDisappear = 0.4f;
    private float timeDelayBeforeResetPosition = 0.9f;
    private float transitionDurationShot = 1.5f;
    private float transitionDurationResetPosition = 2f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        OnTorsoDeathChannel.OnEventRaised += DestroyParts;

        initTorsoPosition = torso.transform.localPosition;
        initFrontArmPosition = frontArm.transform.localPosition;
        initBackArmPosition = backArm.transform.localPosition;
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

    public void MoveToTarget(Vector2 targetPos, Bounds size)
    {
        bool isTargetLowerThanTorso = transform.InverseTransformPoint(targetPos).y < initTorsoPosition.y;
        Vector2 endValue = new Vector2(
            torso.transform.localPosition.x,
            isTargetLowerThanTorso ? transform.InverseTransformPoint(size.max).y : transform.InverseTransformPoint(size.min).y
        );

        if (endValue.y > initTorsoPosition.y)
        {
            endValue.y = initTorsoPosition.y;
        }

        StartCoroutine(MovePartTo(
            frontArm.transform,
            initFrontArmPosition + (Vector2.one * 0.5f),
            transitionDurationShot / 2
        ));
        StartCoroutine(MovePartTo(
            backArm.transform,
            initBackArmPosition + (Vector2.one * 0.5f),
            transitionDurationShot / 2
        ));
        StartCoroutine(MovePartTo(torso.transform, endValue, transitionDurationShot));
        StartCoroutine(ShootLaser());
    }

    IEnumerator MovePartTo(Transform part, Vector2 endPosition, float duration)
    {
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            part.localPosition = Vector2.Lerp(
                part.localPosition,
                endPosition,
                timeElapsed / duration
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        part.localPosition = endPosition;
    }

    IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(timeDelayBeforeShot);
        laser.transform.position = laserFirePoint.position;
        laser.SetActive(true);
        yield return new WaitForSeconds(timeDelayBeforeLaserDisappear);
        laser.SetActive(false);

        yield return new WaitForSeconds(timeDelayBeforeResetPosition);
        StartCoroutine(MovePartTo(frontArm.transform, initFrontArmPosition, transitionDurationResetPosition / 2f));
        StartCoroutine(MovePartTo(backArm.transform, initBackArmPosition, transitionDurationResetPosition / 2f));
        StartCoroutine(MovePartTo(torso.transform, initTorsoPosition, transitionDurationResetPosition));
    }

    Vector2 VectorFromAngle(float angle)
    {
        float angleInRadians = angle * Mathf.Deg2Rad;
        return new Vector2(Mathf.Cos(angleInRadians), Mathf.Sin(angleInRadians));
    }

    private void OnDisable()
    {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
    }
}
