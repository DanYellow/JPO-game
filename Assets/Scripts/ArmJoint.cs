using UnityEngine;

public class ArmJoint : MonoBehaviour
{
    private LineRenderer lineRenderer;

    public Transform bodyJoint;
    public Transform armJoint;

    public float breakDistance = 3f;

    public bool hideOnAwake = false;

    private void Awake()
    {
        lineRenderer = GetComponentInChildren<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        if(hideOnAwake) {
            lineRenderer.material.SetColor("_Color", new Color(1f, 1f, 1f, 0));
        }
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.SetPosition(0, bodyJoint.position);
        lineRenderer.SetPosition(1, armJoint.position);

        lineRenderer.enabled = Vector2.Distance(bodyJoint.position, armJoint.position) <= breakDistance;
    }
}
