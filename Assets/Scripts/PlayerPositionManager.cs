using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionManager : MonoBehaviour
{
    [SerializeField]
    private Transform topLeft;

    [SerializeField]
    private Transform bottomLeft;

    [SerializeField]
    private Transform bottomRight;
    [SerializeField]
    private Transform topRight;

    // Start is called before the first frame update
    private void Awake()
    {
        Mesh planeMesh = GetComponent<MeshFilter>().mesh;
        Bounds bounds = planeMesh.bounds;

        topLeft.localPosition = new Vector3(bounds.min.x, 0, bounds.max.z);
        bottomLeft.localPosition = new Vector3(bounds.min.x, 0, bounds.min.z);

        bottomRight.localPosition = new Vector3(bounds.max.x, 0, bounds.min.z);
        topRight.localPosition = new Vector3(bounds.max.x, 0, bounds.max.z);
    }
}
