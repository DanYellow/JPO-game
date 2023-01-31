using System.Collections;
using System.Collections.Generic;
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

    public bool isLaserAttacking = false;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        OnTorsoDeathChannel.OnEventRaised += DestroyParts;

        initTorsoPosition = torso.transform.localPosition;

        // Debug.Log("localPosition " + torso.transform.localPosition);
        // Debug.Log("position " + torso.transform.position);
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            // animator.SetBool("LaserBeamAttack", true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            // animator.SetBool("Debug", !animator.GetBool("Debug"));
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

    public void ShootBeam(Vector2 targetPos, Bounds size)
    {
        bool isTargetLowerThanTorso = transform.InverseTransformPoint(targetPos).y < initTorsoPosition.y;
        torso.transform.localPosition = new Vector3(
            torso.transform.localPosition.x,
            isTargetLowerThanTorso ? transform.InverseTransformPoint(size.max).y : transform.InverseTransformPoint(size.min).y
        );
        // Debug.Log("size.center " +  size.center);
        /// https://docs.unity3d.com/ScriptReference/Transform.InverseTransformPoint.html
        /// // Debug.Log("dd " + Mathf.Clamp(targetPos.y, 0, initTorsoPosition.y));
        // Debug.Log("localPosition " + targetPos.InverseTransformPoint(torso.transform.localPosition));
        // Debug.Log("size " + torso.GetComponent<SpriteRenderer>().bounds.size.y);
        // Debug.Log("max " + size.max);
        // Debug.Log("InverseTransformPoint " + transform.InverseTransformPoint(size.center));
        Debug.Log("InverseTransformPoint 2 " + transform.InverseTransformPoint(size.max));
        // torso.transform.localPosition = new Vector3(
        //     torso.transform.localPosition.x, 
        //     Mathf.Clamp(targetPos.y, 0, initTorsoPosition.y)
        //     // (torso.transform.position - laserFirePoint.localPosition).y
        // );
        //  transform.parent.position = transform.position - transform.localpostiion;
    }

    public void MoveToTargetProxy(Vector2 targetPos, Bounds size) {
        StartCoroutine(MoveToTarget(targetPos, size));
    }

    IEnumerator MoveToTarget(Vector2 targetPos, Bounds size) {
        float lerpDuration = 1; 

        bool isTargetLowerThanTorso = transform.InverseTransformPoint(targetPos).y < initTorsoPosition.y;
        Vector2 endValue = new Vector2(
            torso.transform.localPosition.x,
            isTargetLowerThanTorso ? transform.InverseTransformPoint(size.max).y : transform.InverseTransformPoint(size.min).y
        );

        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            torso.transform.localPosition = Vector2.Lerp(
                torso.transform.localPosition, 
                endValue, 
                timeElapsed / lerpDuration
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        torso.transform.localPosition = endValue;
        StartCoroutine(ShootLaser());
    }

    IEnumerator ShootLaser()
    {
        yield return new WaitForSeconds(0.25f);
        laser.transform.position = laserFirePoint.position;
        laser.SetActive(true);
        // GameObject laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.45f);
        laser.SetActive(false);
        // animator.SetBool("LaserBeamAttack", false);
        // Destroy(laser);
    }

    private void OnDisable()
    {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
    }
}
