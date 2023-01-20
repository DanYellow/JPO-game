using UnityEngine;

[CreateAssetMenu(fileName = "New Float Value", menuName = "EndlessRunnerJPO/FloatValue", order = 0)]
public class FloatValue : ScriptableObject
{
    public float CurrentValue;

    [Multiline]
    public string DeveloperDescription = "";
}