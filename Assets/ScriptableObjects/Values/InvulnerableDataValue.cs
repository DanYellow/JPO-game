using UnityEngine;

[CreateAssetMenu(fileName = "InvulnerableDataValue", menuName = "ScriptableObjects/Values/InvulnerableDataValue", order = 0)]
public class InvulnerableDataValue : ScriptableObject
{
    [Tooltip("Time flashing between 0 and 1 opacity")]
    public float flashDelay = 0.2f;
    [Tooltip("Time during which the player will be considered as invulnerable")]
    public float time = 0.75f;

    [Multiline, SerializeField]
    private string DeveloperDescription = "";
}