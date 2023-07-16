using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible Var", menuName = "ScriptableObjects/Variable/CollectibleVariable")]
public class CollectibleVariable : ScriptableObject
{
    public int value;
  
    [Multiline]
    public string DeveloperDescription = "";
}