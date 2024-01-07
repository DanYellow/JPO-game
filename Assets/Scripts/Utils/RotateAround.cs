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
    private float rotationSpeed = 150;

    [SerializeField]
    private Transform pivot;

    public bool isAnticlockwise = true; 

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(
            pivot.position, 
            isAnticlockwise ? -pivot.forward : pivot.forward, 
            Time.deltaTime * rotationSpeed
        );
    }
}
