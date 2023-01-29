using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmJoint : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform leftBodyJoint;
    public Transform rightBodyJoint;
    public Transform leftArmJoint;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.useWorldSpace = true;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, leftBodyJoint.position);
        lineRenderer.SetPosition(1, leftArmJoint.position);
    }
}
