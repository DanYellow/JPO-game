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
    private LaserSprite laserSprite;

    [Header("Parts")]
    [SerializeField]
    public GameObject torso;
    private SecretBossTorso secretBossTorso;
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

    // Time delays - shot
    private float timeDelayBeforeShot = 0.7f;
    private float timeDelayBeforeLaserDisappear = 0.4f;
    private float timeDelayBeforeResetPosition = 0.8f;
    private float timeToReachTarget = 0.1f;
    private float transitionDurationResetPosition = 1.75f;
    public bool isReadyToShootLaser = true;

    // Time delays - arms electric
    private float timeDelayLoadLightning = 0.1f;
    private float timeToReachPlayer = 1.35f;
    private float timeBeforeResetArmsPosition = 1.65f;
    private float timeBeforeAttack = 0.65f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        secretBossTorso = torso.GetComponent<SecretBossTorso>();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        laserSprite = laser.GetComponent<LaserSprite>();
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
            ThrowArms(Vector2.zero);
        }
    }

    private void DestroyParts()
    {
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            Destroy(g.gameObject);
        }
    }

    public void MoveToShootTarget(Vector2 targetPos, Bounds targetSize)
    {
        StartCoroutine(MoveToShootCoroutine(targetPos, targetSize));
    }

    IEnumerator MoveToShootCoroutine(Vector2 targetPos, Bounds size)
    {
        isReadyToShootLaser = false;
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
            timeToReachTarget / 2
        ));
        yield return StartCoroutine(MovePartTo(
            backArm.transform,
            initBackArmPosition + (Vector2.one * 0.5f),
            timeToReachTarget / 2
        ));
        yield return StartCoroutine(MovePartTo(torso.transform, endValue, timeToReachTarget * 1.5f));
        StartCoroutine(ShootLaser());
    }

    public void ThrowArms(Vector2 targetPos)
    {
        isReadyToShootLaser = false;
        frontArm.GetComponent<Collider2D>().isTrigger = true;
        StartCoroutine(ThrowArmsCoroutine());
    }

    IEnumerator ThrowArmsCoroutine()
    {
        float speed = (Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height) * 2;
        float originalArmsDistance = Vector3.Distance(backArm.transform.position, frontArm.transform.position);

        // Close arms
        StartCoroutine(MovePartTo(
            frontArm.transform,
            frontArm.transform.localPosition - new Vector3(originalArmsDistance / 4, 0, 0),
            timeDelayLoadLightning
        ));

        yield return StartCoroutine(MovePartTo(
             backArm.transform,
             backArm.transform.localPosition + new Vector3(originalArmsDistance / 4, 0, 0),
             timeDelayLoadLightning
         ));

        yield return new WaitForSeconds(timeDelayLoadLightning / 3);

        lightningAttack.SetActive(true);

        // Go expand arms
        StartCoroutine(MovePartTo(
            frontArm.transform,
            frontArm.transform.localPosition + new Vector3(originalArmsDistance / 4, 0, 0),
            timeDelayLoadLightning / 2
        ));

        yield return StartCoroutine(MovePartTo(
             backArm.transform,
             backArm.transform.localPosition - new Vector3(originalArmsDistance / 4, 0, 0),
             timeDelayLoadLightning / 2
         ));

        yield return new WaitForSeconds(timeBeforeAttack);

        // Go attack player
        StartCoroutine(MovePartTo(
            backArm.transform,
            (Vector3.left * 10),
            timeToReachPlayer
        ));
        yield return StartCoroutine(MovePartTo(
            frontArm.transform,
            (Vector3.left * 10) + new Vector3(originalArmsDistance, 0, 0),
           timeToReachPlayer
        ));

        yield return new WaitForSeconds(timeBeforeResetArmsPosition);
        ResetArmsPosition();
        isReadyToShootLaser = true;
    }

    private void ResetArmsPosition()
    {
        lightningAttack.SetActive(false);
        frontArm.GetComponent<Collider2D>().isTrigger = false;
        frontArm.transform.localPosition = initFrontArmPosition;
        backArm.transform.localPosition = initBackArmPosition;
    }

    IEnumerator MovePartTo(Transform part, Vector2 endPosition, float duration)
    {
        float timeElapsed = 0;
        float lerpValue = 0;
        Vector2 startPosition = part.localPosition;
        while (timeElapsed < duration)
        {
            lerpValue = Mathf.InverseLerp(0, duration, timeElapsed);
            part.localPosition = Vector2.Lerp(
                startPosition,
                endPosition,
                lerpValue
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        part.localPosition = endPosition;
        yield return null;
    }

    IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(timeDelayBeforeShot);
        yield return StartCoroutine(secretBossTorso.ShootLaser());
        yield return new WaitForSeconds(timeDelayBeforeResetPosition);
        StartCoroutine(MovePartTo(frontArm.transform, initFrontArmPosition, timeToReachTarget));
        StartCoroutine(MovePartTo(backArm.transform, initBackArmPosition, timeToReachTarget));
        yield return StartCoroutine(MovePartTo(torso.transform, initTorsoPosition, timeToReachTarget));
        yield return new WaitForSeconds(timeDelayBeforeLaserDisappear);
        yield return new WaitForSeconds(secretBossTorso.secretBossData.laserShootInterval);

        isReadyToShootLaser = true;
        Debug.Log("Ready for next shoot");
    }

    private void OnDisable()
    {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
    }
}
