using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://medium.com/codex/place-objects-on-a-circle-in-unity-in-20-sec-36fb46e22706
// https://www.youtube.com/watch?v=7fdSYc8WElo
// 4*sin(L(0,2 * pi)) 4*cos(L(0,2 * pi))
// https://docs.unity3d.com/Manual/EditingValueProperties.html
// https://forum.unity.com/threads/doubt-with-code-linear-distribution-in-script.1499420/
public class RotateAround : MonoBehaviour
{

    [SerializeField]
    private float rotationSpeed = 1;

    [SerializeField]
    private Transform pivot;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var subtraction = pivot.position - transform.position;
        var direction = (pivot.position - transform.position).normalized;

        transform.RotateAround(pivot.position, pivot.forward, Time.deltaTime * rotationSpeed);
    }
}
