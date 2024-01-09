
using UnityEngine;

[CreateAssetMenu(fileName = "New Blast Effect Data", menuName = "ScriptableObjects/Values/BlastEffectData", order = 0)]
public class BlastEffectData : ScriptableObject
{
    public int damage = 1;

    public int knockbackForce = 0;
    public float scale = 1;

    public GameObject blastEffect;
    public Color blastEffectColor = Color.white;
}
