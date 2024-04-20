using UnityEngine;

[CreateAssetMenu(fileName = "New Float Value", menuName = "ScriptableObjects/Values/FloatValue", order = 0)]
public class FloatValue : ScriptableObject
{
    public float CurrentValue;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}