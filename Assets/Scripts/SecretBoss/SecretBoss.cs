using System.Collections;
using UnityEngine;

public class SecretBoss : MonoBehaviour
{
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
    private VoidEventChannel OnTorsoDeathChannel;

    [SerializeField]
    private VoidEventChannel OnArmDeathChannel;

    // Time delays - shot
    private float timeDelayBeforeResetPosition = 0.8f;
    private float timeToReachTarget = 0.1f;
    public bool isReadyToShootLaser = true;

    // Time delays - arms electric
    private float timeToReachPlayer = 1.35f;
    private float timeBeforeResetArmsPosition = 0.95f;
    private float timeBeforeAttack = 0.65f;
    public bool isReadyToThrowArms = true;
    public bool canThrowArms = true;

    private void Awake()
    {
        secretBossTorso = torso.GetComponent<SecretBossTorso>();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        laserSprite = laser.GetComponent<LaserSprite>();
        OnTorsoDeathChannel.OnEventRaised += DestroyParts;
        OnArmDeathChannel.OnEventRaised += ArmDestroyed;

        lightningAttack.SetActive(false);

        initTorsoPosition = torso.transform.localPosition;
        initFrontArmPosition = frontArm.transform.localPosition;
        initBackArmPosition = backArm.transform.localPosition;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(
                frontArm.transform.GetComponent<PolygonCollider2D>().bounds.min.y
            );
            Debug.Log(
                frontArm.transform.GetComponent<PolygonCollider2D>().bounds.max.y
            );
        }
    }

    private void DestroyParts()
    {
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            Destroy(g.gameObject);
        }
    }

    private void ArmDestroyed()
    {
        Destroy(backArm);
        canThrowArms = false;
    }

    public void MoveToShootTarget(Vector2 targetPos, Bounds targetSize)
    {
        StartCoroutine(MoveToShootCoroutine(targetPos, targetSize));
    }

    IEnumerator MoveToShootCoroutine(Vector2 targetPos, Bounds size)
    {
        isReadyToShootLaser = false;
        isReadyToThrowArms = false;
        bool isTargetLowerThanTorso = transform.InverseTransformPoint(targetPos).y < initTorsoPosition.y;
        Vector2 endValue = new Vector2(
            torso.transform.localPosition.x,
            isTargetLowerThanTorso ? transform.InverseTransformPoint(size.max).y : transform.InverseTransformPoint(size.min).y
        );

        if (endValue.y > initTorsoPosition.y)
        {
            endValue.y = initTorsoPosition.y;
        }

        if (backArm != null)
        {
            frontArm.GetComponent<Collider2D>().isTrigger = true;
            StartCoroutine(MovePartTo(
                frontArm.transform,
                initFrontArmPosition + (Vector2.one * 0.5f),
                secretBossTorso.secretBossData.moveTorsoToReachTargetTime / 2
            ));
            yield return StartCoroutine(MovePartTo(
                backArm.transform,
                initBackArmPosition + (Vector2.one * 0.5f),
                secretBossTorso.secretBossData.moveTorsoToReachTargetTime / 2
            ));
        }
        yield return StartCoroutine(MovePartTo(torso.transform, endValue, timeToReachTarget * 1.5f));
        StartCoroutine(ShootLaser());
    }

    public bool IsTargetInArmsRange(Vector2 targetPos)
    {
        bool isInArmsRange = (
            (float)targetPos.y <= (float)frontArm.transform.GetComponent<PolygonCollider2D>().bounds.max.y &&
            (float)targetPos.y >= (float)frontArm.transform.GetComponent<PolygonCollider2D>().bounds.min.y
        );
        return isInArmsRange;
    }

    public void ThrowArms()
    {
        isReadyToShootLaser = false;
        isReadyToThrowArms = false;
        frontArm.GetComponent<Collider2D>().isTrigger = true;
        StartCoroutine(ThrowArmsCoroutine());
    }

    IEnumerator ThrowArmsCoroutine()
    {
        float speed = (Camera.main.transform.position.x + Camera.main.orthographicSize * Screen.width / Screen.height) * 2;
        float originalArmsDistance = Vector3.Distance(backArm.transform.position, frontArm.transform.position);

        // Load lightning
        StartCoroutine(MovePartTo(
            frontArm.transform,
            frontArm.transform.localPosition - new Vector3(originalArmsDistance / 4, 0, 0),
            secretBossTorso.secretBossData.loadLightiningDuration
        ));

        yield return StartCoroutine(MovePartTo(
             backArm.transform,
             backArm.transform.localPosition + new Vector3(originalArmsDistance / 4, 0, 0),
             secretBossTorso.secretBossData.loadLightiningDuration
         ));

        yield return new WaitForSeconds(secretBossTorso.secretBossData.loadLightiningDuration / 3);

        lightningAttack.SetActive(true);

        // Go expand arms
        StartCoroutine(MovePartTo(
            frontArm.transform,
            frontArm.transform.localPosition + new Vector3(originalArmsDistance / 4, 0, 0),
            secretBossTorso.secretBossData.loadLightiningDuration / 2
        ));

        yield return StartCoroutine(MovePartTo(
             backArm.transform,
             backArm.transform.localPosition - new Vector3(originalArmsDistance / 4, 0, 0),
             secretBossTorso.secretBossData.loadLightiningDuration / 2
         ));

        yield return new WaitForSeconds(secretBossTorso.secretBossData.timeDelayBeforeThrowArms);

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
        yield return new WaitForSeconds(secretBossTorso.secretBossData.armsAttackInterval);
        isReadyToShootLaser = true;
        isReadyToThrowArms = true;
    }

    private void ResetArmsPosition()
    {
        if (canThrowArms)
        {
            lightningAttack.SetActive(false);
            frontArm.GetComponent<Collider2D>().isTrigger = false;
            frontArm.transform.localPosition = initFrontArmPosition;
            backArm.transform.localPosition = initBackArmPosition;
        }
    }

    IEnumerator MovePartTo(Transform part, Vector2 endPosition, float duration)
    {
        float timeElapsed = 0;
        float lerpValue = 0;
        Vector2 startPosition = part.localPosition;
        while (timeElapsed < duration)
        {
            if (part == null)
                yield break;
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
        yield return new WaitForSeconds(secretBossTorso.secretBossData.timeDelayBeforeShoot);
        yield return StartCoroutine(secretBossTorso.ShootLaser());
        yield return new WaitForSeconds(timeDelayBeforeResetPosition);
        if (backArm != null)
        {
            frontArm.GetComponent<Collider2D>().isTrigger = false;
            StartCoroutine(MovePartTo(frontArm.transform, initFrontArmPosition, timeToReachTarget));
            StartCoroutine(MovePartTo(backArm.transform, initBackArmPosition, timeToReachTarget));
        }
        yield return StartCoroutine(MovePartTo(torso.transform, initTorsoPosition, timeToReachTarget));
        yield return new WaitForSeconds(secretBossTorso.secretBossData.laserShootInterval);

        isReadyToShootLaser = true;
        isReadyToThrowArms = true;
    }

    private void OnDisable()
    {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
        OnArmDeathChannel.OnEventRaised -= ArmDestroyed;
    }
}
