using UnityEngine;

[CreateAssetMenu(fileName = "New Collectible Var", menuName = "ScriptableObjects/Variable/CollectibleVariable")]
public class CollectibleVariable : ScriptableObject
{
    public float value;
  
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
}