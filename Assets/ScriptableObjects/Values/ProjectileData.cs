
using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Data", menuName = "ScriptableObjects/Values/ProjectileData", order = 0)]
public class ProjectileData : ScriptableObject
{
    public float speed = 2;
    public float torque = 0.1f;
    public int damage = 1;
    public Vector2 knockback = Vector2.zero;
    [HideInInspector]
    public ShootDirection shootDirection;

    public bool isFacingRight = true;
}
