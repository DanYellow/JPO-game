using System.Collections;
using UnityEngine;
using UnityEngine.Playables;

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
    public SecretBossTorso secretBossTorso;
    [SerializeField]
    private GameObject frontArm;
    private IEnumerator[] listArmsOscillationMovement = new IEnumerator[2];

    [SerializeField]
    private GameObject backArm;
    [SerializeField]
    private PlayableDirector director;

    [SerializeField]
    private GameObject lightningAttack;

    public Vector2 initTorsoPosition {private set; get;}
    private Vector2 initFrontArmPosition;
    private Vector2 initBackArmPosition;

    [SerializeField]
    private VoidEventChannel OnTorsoDeathChannel;

    [SerializeField]
    private VoidEventChannel OnArmDeathChannel;

    // Time delays - shot
    private float timeDelayBeforeResetPosition = 0.8f;
    private float timeToReachTarget = 0.1f;

    [ReadOnlyInspector]
    public bool isReadyToShootLaser = false;

    // Time delays - arms electric
    private float timeToReachPlayer = 1.35f;
    private float timeBeforeResetArmsPosition = 0.95f;
    private float timeBeforeAttack = 0.65f;

    [ReadOnlyInspector]
    public bool isReadyToThrowArms = false;
    public bool canThrowArms = true;

    [ReadOnlyInspector]
    public bool isActivating = false;

    [SerializeField]
    private CinemachineShakeEventChannel onCinemachineShake;

    [SerializeField]
    private ShakeTypeValue bossActivationShake;

    private void Awake()
    {
        secretBossTorso = torso.GetComponent<SecretBossTorso>();
        secretBossTorso.isInvulnerable = true;
        laser = Instantiate(laserPrefab, laserFirePoint.position, laserFirePoint.rotation);
        laser.SetActive(false);
        laserSprite = laser.GetComponent<LaserSprite>();

        lightningAttack.SetActive(false);

        OnArmDeathChannel.OnEventRaised += ArmDestroyed;
        OnTorsoDeathChannel.OnEventRaised += DestroyParts;
        director.stopped += ActivationEnds;
    }

    void ActivationEnds(PlayableDirector obj) {
        StartCoroutine(ActivationEndsCoroutine());
    }

    IEnumerator ActivationEndsCoroutine()
    {
        GetComponent<Animator>().SetTrigger("CombatStarted");
        yield return new WaitForSeconds(0.65f);
        isReadyToShootLaser = true;
        isReadyToThrowArms = true;

        director.enabled = false;

        secretBossTorso.isInvulnerable = false;
        initTorsoPosition = torso.transform.localPosition;
        initFrontArmPosition = frontArm.transform.localPosition;
        initBackArmPosition = backArm.transform.localPosition;

        listArmsOscillationMovement[0] = frontArm.GetComponent<OscillationMovement>().Move();
        listArmsOscillationMovement[1] = backArm.GetComponent<OscillationMovement>().Move();

        // ToggleArmsOscillation(true);
    }

    IEnumerator ToggleArmsOscillation() {
        yield return new WaitForSeconds(0.5f);
        
    }

    private void ToggleArmsOscillation(bool isActivating) {
        // foreach (var armOscillation in listArmsOscillationMovement)
        // {
        //     if(isActivating){
        //         StartCoroutine(armOscillation);
        //     } else {
        //         StopCoroutine(armOscillation);
        //     }
        // }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ThrowArms();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
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
        ToggleArmsOscillation(false);
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
            frontArm.GetComponent<SecretBossArm>().isInvulnerable = true;
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

    public bool IsTargetInLaserRange(Vector2 targetPos)
    {
        bool isInLaserRange = (
            (float)targetPos.y <= (float)torso.transform.GetComponent<BoxCollider2D>().bounds.max.y &&
            (float)targetPos.y >= (float)torso.transform.GetComponent<BoxCollider2D>().bounds.min.y
        );
        return isInLaserRange;
    }

    public void ThrowArms()
    {
        ToggleArmsOscillation(false);
        isReadyToShootLaser = false;
        isReadyToThrowArms = false;
        frontArm.GetComponent<Collider2D>().isTrigger = true;
        frontArm.GetComponent<SecretBossArm>().isInvulnerable = false;
        StartCoroutine(ThrowArmsCoroutine());
    }

    IEnumerator ThrowArmsCoroutine()
    {
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
            ToggleArmsOscillation(true);
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
        yield return new WaitForSeconds(secretBossTorso.secretBossData.timeDelayBeforeShoot / secretBossTorso.phase.factor);
        yield return StartCoroutine(secretBossTorso.ShootLaser());
        yield return new WaitForSeconds(timeDelayBeforeResetPosition);
        if (backArm != null)
        {
            StartCoroutine(MovePartTo(frontArm.transform, initFrontArmPosition, timeToReachTarget));
            StartCoroutine(MovePartTo(backArm.transform, initBackArmPosition, timeToReachTarget));
            frontArm.GetComponent<Collider2D>().isTrigger = false;
            frontArm.GetComponent<SecretBossArm>().isInvulnerable = false;
        }
        yield return StartCoroutine(MovePartTo(torso.transform, initTorsoPosition, timeToReachTarget / secretBossTorso.phase.factor));
        yield return new WaitForSeconds(secretBossTorso.secretBossData.laserShootInterval / secretBossTorso.phase.factor);

        isReadyToShootLaser = true;
        isReadyToThrowArms = true;
        ToggleArmsOscillation(true);
    }

    private void OnDisable()
    {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
        OnArmDeathChannel.OnEventRaised -= ArmDestroyed;
        director.stopped -= ActivationEnds;
    }

    public void StartCombat()
    {
        isActivating = true;
        director.Play();
    }

    public void ActivateShake()
    {
        onCinemachineShake.Raise(bossActivationShake);
    }
}
