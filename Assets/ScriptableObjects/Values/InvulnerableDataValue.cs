using UnityEngine;

[CreateAssetMenu(fileName = "InvulnerableDataValue", menuName = "ScriptableObjects/Values/InvulnerableDataValue", order = 0)]
public class InvulnerableDataValue : ScriptableObject
{
    [Tooltip("Delay before first flash")]
    public float delay = 0;
    [Tooltip("Time flashing between 0 and 1 opacity")]
    public float flashDelay = 0.2f;
    [Tooltip("Duration during which the player will be considered as invulnerable")]
    public float duration = 0.75f;

    [Tooltip("Material to switch with to during invicibility")]
    public Material material;

#pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
#pragma warning restore 0414
}