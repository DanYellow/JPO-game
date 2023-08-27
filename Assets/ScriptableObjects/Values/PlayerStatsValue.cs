
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "ScriptableObjects/Values/PlayerStatsValue", order = 0)]
public class PlayerStatsValue : ScriptableObject
{
    public float moveSpeed;

    public int maxLifePoints = 250;
    public int currentLifePoints;

    public float jumpForce = 5.5f;
    public int maxJumpCount = 1;

    public int gravityScaleGrounded = 2;
    public int gravityScaleFalling = 8;

    [SerializeField]
    public InvulnerableDataValue invulnerableData;

    private void OnEnable() {
        currentLifePoints = 20;
        // currentLifePoints = maxLifePoints;
    }
}
