using UnityEngine;
using System;

public class FloatBehavior : MonoBehaviour
{
    [SerializeField]
    private EnemyStatsValue enemyStatsValue;
    private float originalY;

    private void Awake()
    {
        originalY = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(
            transform.position.x,
            originalY + ((float)Math.Sin(Time.time) * (enemyStatsValue?.floatStrength ?? 0.2f)),
            transform.position.z
        );
    }
}