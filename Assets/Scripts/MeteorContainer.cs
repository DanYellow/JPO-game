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
        // meteorEffect.gameObject.SetActive(false);
        foreach (Transform child in transform)
        {
            child.localPosition = Vector3.zero;
            child.localRotation = Quaternion.identity;
            if (child.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                // print("ffffa");
                rb.MovePosition(Vector3.zero);
                // rb.position = Vector3.zero;
            }
        }
        meteor.ResetThyself();
    }
}
