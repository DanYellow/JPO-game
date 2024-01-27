using UnityEngine;

[CreateAssetMenu(fileName = "New Vector2 Value", menuName = "ScriptableObjects/Values/Vector2Value", order = 0)]
public class Vector2Value : ScriptableObject
{
    public Vector2? CurrentValue = null;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}