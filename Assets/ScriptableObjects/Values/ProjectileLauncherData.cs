
using UnityEngine;

public enum ShootDirection {
    Right,
    Left
}

[CreateAssetMenu(fileName = "New Projectile Launcher Data", menuName = "ScriptableObjects/Values/ProjectileLauncherData", order = 0)]
public class ProjectileLauncherData : ScriptableObject
{
    public Projectile projectile;
    public float cadency;
    public float startDelay = 0;
    public ShootDirection shootDirection;
}
