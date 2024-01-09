using UnityEngine;

[CreateAssetMenu(fileName = "New Projectile Data", menuName = "ScriptableObjects/Values/ProjectileData", order = 0)]
public class ProjectileData : ScriptableObject
{
    public float speed = 2;
    public float torque = 0.1f;
    public int damage = 1;
    public int knockbackForce = 0;
    public bool isFacingRight = true;

    public GameObject blastEffect;
    public Color blastEffectColor = Color.white;
}
