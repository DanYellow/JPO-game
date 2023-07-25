using UnityEngine;

[CreateAssetMenu(fileName = "MaterialChangeValue", menuName = "ScriptableObjects/Values/MaterialChangeValue", order = 0)]
public class MaterialChangeValue : ScriptableObject
{
    [Tooltip("Material to switch to during the flash.")]
    public Material material;
    public float interval;
    public float duration;

    [Range(0, 1)]
    public float opacity = 1;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}