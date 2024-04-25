using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorImpact : MonoBehaviour
{
    [SerializeField]
    private Transform world;
    // Start is called before the first frame update
    void Awake()
    {
        gameObject.SetActive(false);
        transform.LookAt(world);
    }

    private void OnTriggerEnter(Collider other) {
        
    }
}
