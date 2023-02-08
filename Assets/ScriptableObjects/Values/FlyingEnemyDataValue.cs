using UnityEngine;

[CreateAssetMenu(fileName = "FlyingEnemyDataValue", menuName = "EndlessRunnerJPO/FlyingEnemyDataValue", order = 0)]
public class FlyingEnemyDataValue : EnemyStatsValue {
    [Range(0.1f, 1.5f)]
    public float flySpeed = 0.65f;
}