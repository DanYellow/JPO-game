public interface IDamageable {
    bool isSensitiveToLava { get; set; }
    void TakeDamage(float damage);
}
