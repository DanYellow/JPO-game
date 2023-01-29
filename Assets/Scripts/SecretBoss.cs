using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBoss : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Transform laserFirePoint;

    [SerializeField]
    private GameObject laser;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)) {
            animator.SetBool("LaserBeamAttack", true);
        }
    }

    public void ShootLaser() {
        Instantiate(laser, laserFirePoint.position, Quaternion.identity);
    }
}
