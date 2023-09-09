
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

    public float distanceDetector = 0;

    public float obstacleCheckRadius = 0.25f;

    public int knockbackForce = 0;

    public GameObject dropItem;
    public GameObject blastEffect;
    [Range(0, 1)]
    public float dropProbability = 0.25f;
}
