using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlying : Enemy
{
    public Transform target;
    private Vector2 startingPosition;
    public bool isChasing = false;
    private Vector2 nextDirection;

    private FlyingEnemyDataValue enemyFlyingData;

    public override void Awake()
    {
        base.Awake();
        startingPosition = transform.position;
        nextDirection = startingPosition;
        rb = GetComponent<Rigidbody2D>();
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
        nextDirection = target.position;
        transform.position = Vector2.MoveTowards(transform.position, target.position, enemyFlyingData.flySpeed * Time.deltaTime);
    }

    void ReturnToStartPoint()
    {
        nextDirection = startingPosition;
        transform.position = Vector2.MoveTowards(transform.position, startingPosition, (enemyFlyingData.flySpeed * 1.25f) * Time.deltaTime);
    }

    void Flip()
    {
        if(transform.position.x > nextDirection.x) {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
        }
    }
}
