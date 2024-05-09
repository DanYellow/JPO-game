using UnityEngine;

[CreateAssetMenu(fileName = "New String Value", menuName = "ScriptableObjects/Values/StringValue", order = 0)]
public class StringValue : ScriptableObject
{
    public string CurrentValue;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}