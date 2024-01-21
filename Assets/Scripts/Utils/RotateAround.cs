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
    private float baseSpeed;

    [SerializeField]
    private Transform pivot;

    public bool isAnticlockwise = true;

    private void Awake()
    {
        baseSpeed = rotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (pivot != null)
        {
            transform.RotateAround(
                pivot.position,
                isAnticlockwise ? -transform.forward : transform.forward,
                Time.deltaTime * rotationSpeed
            );
        }
    }

    public float GetSpeed()
    {
        return rotationSpeed;
    }

    public float GetBaseSpeed()
    {
        return baseSpeed;
    }

    public void SetSpeed(float speed)
    {
        rotationSpeed = speed;
    }
}
