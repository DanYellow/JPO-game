using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
    [SerializeField]
    private BoolValue isCarTakingDamage;

    // Start is called before the first frame update
    void Start()
    {
        isCarTakingDamage.CurrentValue = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
