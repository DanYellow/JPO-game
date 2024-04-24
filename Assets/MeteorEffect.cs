using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorEffect : MonoBehaviour
{
    void OnParticleCollision(GameObject other)
    {
        print("other.transform.name " + other.transform.name);
    }
}
