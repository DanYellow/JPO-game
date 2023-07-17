using UnityEngine;

[CreateAssetMenu(fileName = "MaterialChangeValue", menuName = "ScriptableObjects/Values/MaterialChangeValue", order = 0)]
public class MaterialChangeValue : ScriptableObject
{
    [Tooltip("Material to switch to during the flash.")]
    public Material material;
    public float interval;
    public float duration;

    [Multiline, SerializeField]
    private string DeveloperDescription = "";
}