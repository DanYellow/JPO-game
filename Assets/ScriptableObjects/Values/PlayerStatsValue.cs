
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "EndlessRunnerJPO/PlayerStatsValue", order = 0)]
public class PlayerStatsValue : ScriptableObject
{
    public float maxHealth;
    public float currentHealth;
    public float moveSpeed;
    public float jumpForce;
    public int maxJumpCount = 1;

    [Range(0, 5)]
    public float knockbackForce = 1.75f;

    public float speedFactor = 1;
    public float waterSpeedFactor = 0.5f;

    public float fallThreshold = -5f;

    public float damage = 1;

    public float invulnerabiltyTime = 2.5f;

    private void OnEnable() {
        currentHealth = maxHealth;
    }
}
