using UnityEngine;

[CreateAssetMenu(fileName = "FlyingEnemyDataValue", menuName = "EndlessRunnerJPO/FlyingEnemyDataValue", order = 0)]
public class FlyingEnemyDataValue : EnemyStatsValue {
    [Range(0.1f, 1.5f)]
    public float flySpeed = 0.65f;
    public float dashingRange = 3f;
    public float delayBeforeRestartDashing = 1.25f;
}