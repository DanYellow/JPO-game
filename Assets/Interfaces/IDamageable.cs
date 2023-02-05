public interface IDamageable {
    bool isSensitiveToLava { get; set; }
    bool isInvulnerable { get; set; }
    void TakeDamage(float damage);
}
