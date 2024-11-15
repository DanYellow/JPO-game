
using UnityEngine;

[CreateAssetMenu(fileName = "New Player Data", menuName = "ScriptableObjects/Values/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    [Range(1, 15), HideInSubClass]
    public float jumpForce = 7;

    [Range(0, 1), HideInSubClass]
    public float groundPoundCooldown = 0.65f;

    [Range(5, 20), HideInSubClass]
    public float dropForce = 10;

    [HideInSubClass]
    public float groundCheckRadius = 0.15f;

    [HideInSubClass]
    public int maxNbLives = 3;
    [HideInSubClass]
    public float invincibilityTime = 1.25f;

    [HideInSubClass]
    public GameObject waveEffect;

    [HideInSubClass]
    public LayerMask damageLayer;
}