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
    private GameObject torso;
    [SerializeField]
    private GameObject frontArm;
    [SerializeField]
    private GameObject backArm;

    [SerializeField]
    VoidEventChannel OnTorsoDeathChannel;


    private void Awake() {
        animator = GetComponent<Animator>();
        OnTorsoDeathChannel.OnEventRaised += DestroyParts;
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            animator.SetBool("LaserBeamAttack", true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            animator.SetBool("Debug", !animator.GetBool("Debug"));
            frontArm.transform.position += new Vector3(2, 0, 0);
        }
    }

    private void DestroyParts() {
        foreach (Transform g in GetComponentsInChildren<Transform>())
        {
            Destroy(g.gameObject);
        }
    }

    IEnumerator ShootLaser()
    {
        GameObject laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("LaserBeamAttack", false);
        Destroy(laser);
    }

    private void OnDisable() {
        OnTorsoDeathChannel.OnEventRaised -= DestroyParts;
    }
}
