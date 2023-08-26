
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "ScriptableObjects/Values/PlayerStatsValue", order = 0)]
public class PlayerStatsValue : ScriptableObject
{
    public float moveSpeed;

    [Range(1, 99)]
    public int nbMaxLifes = 3;
    public int nbCurrentLifes;

    public float jumpForce = 5.5f;
    public int maxJumpCount = 2;

    public int gravityScaleGrounded = 2;
    public int gravityScaleFalling = 8;

    [SerializeField]
    public InvulnerableDataValue invulnerableData;

    private void OnEnable() {
        nbCurrentLifes = nbMaxLifes;
    }
}
