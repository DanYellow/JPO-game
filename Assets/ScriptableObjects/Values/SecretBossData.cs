using UnityEngine;
using System;

[CreateAssetMenu(fileName = "SecretBossData", menuName = "EndlessRunnerJPO/SecretBossData", order = 0)]
public class SecretBossData : EnemyStatsValue
{
    public Phase[] listPhases;
    public float laserDamage;

    public float laserRetractTime = 1f;
    public float laserShootInterval = 4f;

    void OnValidate()
    {
        laserDamage = Mathf.Max(laserDamage, 1);
    }
}

[Serializable]
public class Phase
{
    public float threshold;
    public Sprite sprite;

    [Range(1, 2)]
    public float attackDamageFactor;


}