using UnityEngine;

[CreateAssetMenu(fileName = "JumperDataValue", menuName = "EndlessRunnerJPO/JumperDataValue", order = 0)]
public class JumperDataValue : ScriptableObject {
    [Tooltip("How long it takes to reach the target")]
    public float jumpHigh = 0;
    public float delayBetweenJumps = 2.5f;
}