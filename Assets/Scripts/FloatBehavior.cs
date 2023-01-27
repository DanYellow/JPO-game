using UnityEngine;
using System;

public class FloatBehavior : MonoBehaviour
{
    [SerializeField]
    private FloatDataValue floatData;
    private float originalY;

    private void Awake()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            originalY + (Mathf.Sin(Time.time) * floatData.floatStrength),
            transform.position.z
        );
    }
}