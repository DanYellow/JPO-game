
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "EndlessRunnerJPO/EnemyStatsValue", order = 0)]
public class EnemyStatsValue : ScriptableObject
{
    public float maxHealth;
    public float damage;
    public float moveSpeed;
    [Tooltip("How much the speed will increase when the player is detected")]
    public float moveSpeedFactor = 1.25f;

    [Tooltip("How much the damage will be reduced on hit")]
    public float defense = 1;

    [Range(0, 5)]
    public float knockbackForce = 1.75f;

    public float attackRange = 0.5f;

    [Tooltip("Which threshold of life point the enemy have to reach in order to enrage")]
    [Range(0, 1)]
    public float enrageThreshold = 0;
    [Range(1, 3)]
    public float enrageFactor = 1;

    public float beamSpeed = 1.75f;
}
