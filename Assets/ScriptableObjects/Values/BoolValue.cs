using UnityEngine;

[CreateAssetMenu(fileName = "New Bool Value", menuName = "ScriptableObjects/Values/BoolValue", order = 0)]
public class BoolValue : ScriptableObject
{
    public bool CurrentValue;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}