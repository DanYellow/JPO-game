using UnityEngine;

public class MeteorContainer : MonoBehaviour
{
    private Meteor meteor;
    private MeteorEffectV2 meteorEffect;

    private void Awake()
    {
        meteor = GetComponentInChildren<Meteor>(true);
        meteorEffect = GetComponentInChildren<MeteorEffectV2>(true);
    }

    public void ResetThyself()
    {
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            if (child.TryGetComponent(out Rigidbody rb))
            {
                rb.MovePosition(Vector3.zero);
            }
        }
        meteorEffect.gameObject.SetActive(false);
        meteor.ResetThyself();

    }
}
