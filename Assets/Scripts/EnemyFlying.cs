using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : Enemy
{
    public Transform target;
    private Vector2 startingPosition;

    [HideInInspector]
    public bool isChasing = false;
    private Vector2 nextDirection;
    private bool isDashing = false;
    private bool startFreeMovement = true;
    public Behaviour[] listDisabledBehaviours;

    private FlyingEnemyDataValue enemyFlyingData;

    public override void Awake()
    {
        base.Awake();
        startingPosition = transform.position;
        nextDirection = startingPosition;
        enemyFlyingData = (FlyingEnemyDataValue)enemyData;
    }

    void Update()
    {
        if (isChasing)
        {
            Chase();
        }
        else
        {
            ReturnToStartPoint();
        }
        Flip();
    }

    void Chase()
    {
        foreach (Behaviour component in listDisabledBehaviours)
        {
            component.enabled = false;
        }
        startFreeMovement = false;

        nextDirection = target.position;
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemyFlyingData.flySpeed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < enemyFlyingData.dashingRange)
        {
            isDashing = true;
            transform.position = Vector2.MoveTowards(transform.position, target.position, enemyFlyingData.flySpeed * 6f * Time.deltaTime);
        }
        else
        {
            isDashing = false;
        }
    }

    void ReturnToStartPoint()
    {
        nextDirection = startingPosition;
        if ((Vector2)transform.position != startingPosition && !startFreeMovement)
        {
            if ((Vector2)transform.position == startingPosition)
            {
                startFreeMovement = true;
                foreach (Behaviour component in listDisabledBehaviours)
                {
                    component.enabled = false;
                }
            }

            transform.position = Vector2.MoveTowards(transform.position, startingPosition, (enemyFlyingData.flySpeed * 1.25f) * Time.deltaTime);
        }
        else
        {
            foreach (Behaviour component in listDisabledBehaviours)
            {
                component.enabled = true;
            }
        }
    }

    void Flip()
    {
        if (!Mathf.Approximately(transform.position.x, nextDirection.x) && transform.position.x > nextDirection.x)
        {
        
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable) && other.transform.CompareTag("Player") && isDashing)
        {
            TakeDamage(float.MaxValue);
            iDamageable.TakeDamage(enemyData.damage);
        }
    }
}
