using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SecretBossData", menuName = "EndlessRunnerJPO/SecretBossData", order = 0)]
public class SecretBossData : EnemyStatsValue
{
    // public Phase currentPhase;
    public Phase[] listPhases;

    [Header("Laser")]
    public float laserDamage;
    public float laserRetractTime = 1f;
    public float laserShootInterval = 4f;
    public float laserAttackRange;
    public float moveTorsoToReachTargetTime = 0.23f;
    public float timeDelayBeforeShoot = 1.35f;

    [Header("Arms")]
    public float armsAttackRange;
    public float armsAttackInterval = 4f;
    public float timeDelayBeforeThrowArms = 0.82f;
    public float loadLightiningDuration = 0.07f;

    public InvulnerableDataValue invulnerableData;

    private void Awake() {
        
    }

    void OnValidate()
    {
        laserDamage = Mathf.Max(laserDamage, 1);
        laserAttackRange = Mathf.Max(laserAttackRange, 1);
        armsAttackRange = Mathf.Max(armsAttackRange, 1);
    }
}

[Serializable]
public class Phase
{
    public float threshold;
    public Sprite sprite;

    [Range(1, 2), UnityEngine.Serialization.FormerlySerializedAs("attackDamageFactor")]
    public float factor;
}