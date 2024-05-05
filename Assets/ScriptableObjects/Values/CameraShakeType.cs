using UnityEngine;

[CreateAssetMenu(fileName = "New Camera Shake Type", menuName = "ScriptableObjects/Values/ShakeType", order = 0)]
public class CameraShakeType : ScriptableObject
{
    public float duration = 2f;
    public float intensity = 3f;

    #pragma warning disable 0414
    [Multiline, SerializeField]
    private string DeveloperDescription = "";
    #pragma warning restore 0414
}