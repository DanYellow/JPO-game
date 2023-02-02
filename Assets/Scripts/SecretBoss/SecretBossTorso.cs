using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    [HideInInspector]
    public bool isInvulnerable = false;
    public bool isReadyToShoot = true;

    private float nextShootTime = 0f;

    public Phase phase { get; private set; }
    private Phase lastPhase;

    public override void Awake()
    {
        base.Awake();
        laser = Instantiate(laserPrefab, laserFirePoint.position, Quaternion.identity);
        laser.SetActive(false);
        laserSprite = laser.GetComponent<LaserSprite>();

        secretBossData = (SecretBossData)enemyData;

    }

    private void Update()
    {
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + secretBossData.laserShootInterval;
        }
    }

    public IEnumerator ShootLaser()
    {
        float damage = secretBossData.laserDamage;
        if(lastPhase != phase) {
            damage *= secretBossData.attackDamageFactor;
            Debug.Log("damage " + damage);
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
        // if (isInvulnerable) return;

        base.TakeDamage(damage);


        phase = GetPhase();
        if(lastPhase != phase) {
            lastPhase = phase;
        }
        
        if (phase != null)
        {
            sr.sprite = phase.sprite;
        }
    }

    private Phase GetPhase()
    {
        // https://stackoverflow.com/questions/22830497/findindex-on-list-by-linq
        List<Phase> someList = new List<Phase>(secretBossData.listPhases);
        someList.ForEach(p => Debug.Log(p.threshold));
        // Debug.Log("item.threshold" + secretBossData.listPhases);
        int indexPhase = someList.FindIndex(item => {
        // Debug.Log("currentHealth / secretBossData.maxHealth " + currentHealth / secretBossData.maxHealth);
        // Debug.Log("item.threshold" + item.threshold);
            return currentHealth / secretBossData.maxHealth < item.threshold;
        });
        // Debug.Log("indexPhase " + indexPhase);
        return (indexPhase < 0 ? null : someList[indexPhase]);
    }
}
