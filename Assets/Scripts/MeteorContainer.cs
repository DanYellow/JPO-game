using UnityEngine;

public class MeteorContainer : MonoBehaviour
{
    private Meteor meteor;
    private MeteorEffectV2 meteorEffect;

    private void Awake()
    {
        meteor = GetComponentInChildren<Meteor>();
        meteorEffect = GetComponentInChildren<MeteorEffectV2>();
    }

    public void ResetThyself()
    {
        meteor.ResetThyself();
        meteorEffect.gameObject.SetActive(false);
    }
}
