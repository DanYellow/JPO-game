using UnityEngine;

public class FishEnemy : Enemy
{
    [Header("Components to enable on FishReady")]
    public Behaviour[] listDisabledBehaviours;

    private Enemy enemy;

    public override void Awake() {
        base.Awake();
        isInvulnerable = true;
    }

    public void OnAnimationEnd() {
        isInvulnerable = false;
        foreach (Behaviour component in listDisabledBehaviours)
        {
            component.enabled = true;
        }
    }

    public override void TakeDamage(float damage)
    {
        if (isInvulnerable) return;
        base.TakeDamage(damage);
    }
}
