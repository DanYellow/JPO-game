using UnityEngine;

[CreateAssetMenu(fileName = "New Float Value", menuName = "ScriptableObjects/FloatValue", order = 0)]
public class FloatValue : ScriptableObject
{
    public float CurrentValue;

    [Multiline, SerializeField]
    private string DeveloperDescription = "";
}