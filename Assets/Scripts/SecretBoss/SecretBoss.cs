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

    [SerializeField]
    private GameObject lightningAttack;


    private Vector2 initTorsoPosition;
    private Vector2 initFrontArmPosition;
    private Vector2 initBackArmPosition;

    [SerializeField]
    VoidEventChannel OnTorsoDeathChannel;

    // Time delays
    private float timeDelayBeforeShot = 0.7f;
    private float timeDelayBeforeLaserDisappear = 0.4f;
    private float timeDelayBeforeResetPosition = 0.8f;
    private float transitionDurationShot = 1.5f;
    private float transitionDurationResetPosition = 1.75f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        OnTorsoDeathChannel.OnEventRaised += DestroyParts;

        lightningAttack.SetActive(false);

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
            ThrowArmsProxy(Vector2.zero);
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

    public void MoveToShootTarget(Vector2 targetPos, Bounds size)
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

    public void ThrowArmsProxy(Vector2 targetPos)
    {
        frontArm.GetComponent<Collider2D>().isTrigger = true;
        StartCoroutine(ThrowArms(targetPos));
    }

    IEnumerator ThrowArms(Vector2 targetPos) {
        float speed = (Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height) * 2;
        float originalArmsDistance = Vector3.Distance(backArm.transform.position, frontArm.transform.position);
        StartCoroutine(MovePartTo(
            frontArm.transform,
            frontArm.transform.localPosition - new Vector3(originalArmsDistance / 4, 0, 0),
            transitionDurationShot / 3
        ));

        StartCoroutine(MovePartTo(
            backArm.transform,
            backArm.transform.localPosition + new Vector3(originalArmsDistance / 4, 0, 0),
            transitionDurationShot / 3
        ));

        yield return new WaitForSeconds(0.75f);

        lightningAttack.SetActive(true);

        StartCoroutine(MovePartTo(
            frontArm.transform,
            frontArm.transform.localPosition + new Vector3(originalArmsDistance / 4, 0, 0),
            transitionDurationShot / 3
        ));

        StartCoroutine(MovePartTo(
            backArm.transform,
            backArm.transform.localPosition - new Vector3(originalArmsDistance / 4, 0, 0),
            transitionDurationShot / 3
        ));

        yield return new WaitForSeconds(1.5f);


        StartCoroutine(MovePartTo(
            frontArm.transform,
            Vector3.left * speed,
            transitionDurationShot * 35
        ));
        
        yield return StartCoroutine(MovePartTo(
            backArm.transform,
            Vector3.left * (speed + Vector3.Distance(backArm.transform.position, frontArm.transform.position)),
            transitionDurationShot * 35
        ));
        backArm.SetActive(false);
        Debug.Log("ttee");
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
        yield break;    
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

    private void OnDisable()
    {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
    }
}
