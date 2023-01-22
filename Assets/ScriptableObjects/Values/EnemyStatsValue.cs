
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "EndlessRunnerJPO/EnemyStatsValue", order = 0)]
public class EnemyStatsValue : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    public float moveSpeedFactor = 1.25f;
}
