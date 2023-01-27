using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BeamValue", menuName = "EndlessRunnerJPO/BeamValue", order = 0)]
public class BeamValue : ScriptableObject {
    public float damage = 0;
    public float moveSpeed = 0;
}