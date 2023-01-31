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

    private void FixedUpdate()
    {
        // Vector2 nextPosition = new Vector2(target.position.x, rb.position.y);
        // rb.MovePosition(
        //     Vector2.MoveTowards(rb.position, nextPosition, 1 * Time.fixedDeltaTime)
        // );
    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
    }

    private void OnBecameInvisible()
    {
        if(enabled) {
            // StartCoroutine(ResetPosition());
        }
    }

    IEnumerator ResetPosition()
    {
        yield return new WaitForSeconds(1.25f);
        rb.velocity = Vector3.zero;
        transform.position = initPosition;
    }
}
