using UnityEngine;

[CreateAssetMenu(fileName = "New Shake Type", menuName = "EndlessRunnerJPO/ShakeTypeValue", order = 0)]
public class ShakeTypeValue : ScriptableObject
{
    public float time = 2f;
    public float intensity = 3f;

    [Multiline]
    public string DeveloperDescription = "";
}