using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
    [Header("Scriptable Objects")]
    [SerializeField]
    private BoolValue isCarTakingDamage;

    [SerializeField]
    private BoolValue hasReachMinimumTravelDistance;

    // Start is called before the first frame update
    void Start()
    {
        isCarTakingDamage.CurrentValue = false;
        hasReachMinimumTravelDistance.CurrentValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
