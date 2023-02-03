using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretBossArm : Enemy
{
    [SerializeField]
    private Transform target;

    private Vector2 initPosition;

    private bool isVisible = true;

    public override void Awake()
    {
        base.Awake();
        initPosition = transform.position;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            rb.velocity += new Vector2(-5, 0);
        }

        if (!isVisible)
        {
            rb.velocity = Vector3.zero;
            transform.position = initPosition;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        base.TakeDamage(damage);
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(1.25f);
        rb.velocity = Vector3.zero;
        transform.position = initPosition;
    }
}
