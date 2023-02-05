using UnityEngine;

public class SecretBossArm : Enemy
{
    [ReadOnlyInspector, SerializeField]
    public bool _isInvulnerable;

    private void Update() {
        _isInvulnerable = isInvulnerable;
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        base.TakeDamage(damage);
    }
}
