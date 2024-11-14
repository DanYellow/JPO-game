
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "ScriptableObjects/Values/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [Range(1, 15)]
    public float jumpForce = 7;

    [Range(0, 1)]
    public float groundPoundCooldown = 0.65f;

    [Range(5, 20)]
    public float dropForce = 10;

    public float groundCheckRadius = 0.15f;

    public int nbLives = 3;

    public GameObject waveEffect;
}