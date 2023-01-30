using UnityEngine;

public class ArmJoint : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform bodyJoint;
    public Transform armJoint;

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
        lineRenderer.SetPosition(0, bodyJoint.position);
        lineRenderer.SetPosition(1, armJoint.position);
    }
}
