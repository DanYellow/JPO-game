using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue isCarTakingDamage;

    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    void Start()
    {
        Application.targetFrameRate = 60;

        isCarTakingDamage.CurrentValue = false;
        hasReachMinimumTravelDistance.CurrentValue = false;
    }
}
