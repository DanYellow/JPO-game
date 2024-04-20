using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody motor;

    [SerializeField]
    private Rigidbody collision;

    [SerializeField]
    private float speed;
    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private GameObject[] listWheels;

    [SerializeField]
    private float rotationWheelSpeed = 150;

    [SerializeField]
    private float steerAngle = 19.5f;

    [SerializeField]
    private RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
