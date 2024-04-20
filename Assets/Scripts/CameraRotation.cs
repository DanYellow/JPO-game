using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame -transform.forward
    void Update()
    {
        transform.rotation = Quaternion.FromToRotation(new Vector3( 
            -transform.forward.x,
            -transform.forward.y,
            -transform.forward.z
        ), target.position) * transform.rotation;
    }
}
