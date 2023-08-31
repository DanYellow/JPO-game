
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Stats", menuName = "ScriptableObjects/Values/EnemyStats", order = 0)]
public class EnemyStats : ScriptableObject
{
    public int maxLifePoints = 25;
    [Range(0, 9)]
    public float walkSpeed = 3;
}
