using UnityEngine;

[CreateAssetMenu(fileName = "InvulnerableDataValue", menuName = "EndlessRunnerJPO/InvulnerableDataValue", order = 0)]
public class InvulnerableDataValue : ScriptableObject
{
    [Tooltip("Time flashing between 0 and 1 opacity")]
    public float invulnerableFlashDelay = 0.2f;
    [Tooltip("Time during which the player will be considered as invisible")]
    public float invulnerabiltyTime = 0.75f;
}