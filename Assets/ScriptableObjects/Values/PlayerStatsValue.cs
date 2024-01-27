
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "ScriptableObjects/Values/PlayerStatsValue", order = 0)]
public class PlayerStatsValue : CharacterData
{
    public int currentLifePoints;
    [Space(20)]
    public float moveSpeed;
    [Space(20)]

    public float jumpForce = 5.5f;
    public int maxJumpCount = 1;

    [Space(20)]
    public int dashVelocity = 30;
    public int dashDamage = 2;
    public int dashCooldown = 3;
    [Space(20)]

    public int gravityScaleGrounded = 2;
    public int gravityScaleFalling = 8;

    [SerializeField]
    public InvulnerableDataValue invulnerableData;

    private void OnEnable() {
        if(currentLifePoints <= 0) {
            currentLifePoints = 1;
        }
        // currentLifePoints = maxLifePoints;
    }

    void Reset()
    {
        currentLifePoints = 20;
    }
}
