
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Data", menuName = "ScriptableObjects/Values/EnemyData", order = 0)]
public class EnemyData : ScriptableObject
{
    public int maxLifePoints = 25;
    [Range(0, 9)]
    public float walkSpeed = 3;

    public float attackRate = 1;
    public int damage = 1;

    public new string name = "";

    public float obstacleCheckRadius = 0.25f;

    public int knockbackForce = 0;
}
