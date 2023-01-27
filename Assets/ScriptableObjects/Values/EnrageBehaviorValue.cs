using UnityEngine;

[CreateAssetMenu(fileName = "EnrageBehaviorValue", menuName = "EndlessRunnerJPO/EnrageBehaviorValue", order = 0)]
public class EnrageBehaviorValue : ScriptableObject
{
    [Range(0, 1), Tooltip("Which threshold of life point the enemy have to reach in order to enrage")]
    public float threshold = 0;

    [Range(1, 3), Tooltip("How much the enemy stats increase when enraged")]
    public float bonusFactor = 1;
}