using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Shake Type", menuName = "ScriptableObjects/Values/ShakeTypeValue", order = 0)]
public class CameraShakeTypeValue : ScriptableObject
{
    public float duration = 2f;
    public float intensity = 3f;

    [Multiline, SerializeField]
    private string DeveloperDescription = "";
}