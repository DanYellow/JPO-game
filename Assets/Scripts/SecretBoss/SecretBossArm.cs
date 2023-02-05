using UnityEngine;

public class SecretBossArm : Enemy
{
    [SerializeField]
    private Transform target;

    private Vector2 initPosition;

    private bool isVisible = true;

    private new Collider2D collider2D;

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
}
