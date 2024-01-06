using System.Collections.Generic;
using UnityEngine;

public class DisabledOnAwake : MonoBehaviour
{
    public bool isActive = false;

    public List<GameObject> listRelatives = new List<GameObject>();
    private void Awake() {
        gameObject.SetActive(isActive);
        listRelatives.ForEach((item) => item.SetActive(isActive));
    }
}
