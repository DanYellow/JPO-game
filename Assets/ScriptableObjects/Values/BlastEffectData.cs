
using UnityEngine;

[CreateAssetMenu(fileName = "New Blast Effect Data", menuName = "ScriptableObjects/Values/BlastEffectData", order = 0)]
public class BlastEffectData : ScriptableObject
{
    public int damage = 1;

    public int knockbackForce = 0;
}
