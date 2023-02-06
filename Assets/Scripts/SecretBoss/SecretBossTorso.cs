using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SecretBossTorso : Enemy
{
    [SerializeField]
    private Transform laserFirePoint;

    [SerializeField]
    private GameObject laserPrefab;
    private GameObject laser;
    private LaserSprite laserSprite;

    [HideInInspector]
    public SecretBossData secretBossData;

    public Phase phase { get; private set; }

    public override void Awake()
    {
        base.Awake();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        laserSprite = laser.GetComponent<LaserSprite>();

        secretBossData = (SecretBossData)enemyData;
        phase = GetPhase();
    }

    public IEnumerator ShootLaser()
    {
        float damage = secretBossData.laserDamage;
        if (phase != null)
        {
            damage *= phase.factor;
        }
        laserSprite.damage = damage;
        laser.transform.position = laserFirePoint.position;
        laser.SetActive(true);
        yield return new WaitForSeconds(secretBossData.laserRetractTime);
        laser.SetActive(false);
        yield return null;
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable) return;

        phase = GetPhase();
        // secretBossData.currentPhase = phase;

        if (phase != null)
        {
            base.TakeDamage(damage / phase.factor);
            sr.sprite = phase.sprite;
        } else {
            base.TakeDamage(damage);
        }
    }

    public override void OnDeath()
    {
        base.OnDeath();
        Destroy(laser);
    }

    private Phase GetPhase()
    {
        List<Phase> phasesList = new List<Phase>(secretBossData.listPhases);

        int indexPhase = phasesList.FindLastIndex(item =>
        {
            return currentHealth / secretBossData.maxHealth <= item.threshold;
        });
        return (indexPhase < 0 ? null : phasesList[indexPhase]);
    }
}
