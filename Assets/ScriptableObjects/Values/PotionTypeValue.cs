using UnityEngine;

public enum PotionType
{
    Heal,
}


[CreateAssetMenu(fileName = "New PotionTypeValue", menuName = "ScriptableObjects/Values/PotionTypeValue", order = 0)]
public class PotionTypeValue : ScriptableObject
{
    public PotionType potionType;
    public int value;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}