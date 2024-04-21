using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float delay = 0;

    private Vector3 offset;
    private void Start() {
        offset = (target.position - transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.rotation, delay * Time.deltaTime);

        transform.position = target.position;
    }
}
