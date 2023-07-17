using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible Var", menuName = "ScriptableObjects/Values/CollectibleVariable")]
public class CollectibleVariable : ScriptableObject
{
    public float value;
  
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
}