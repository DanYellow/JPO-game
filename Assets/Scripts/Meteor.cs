using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private GameObject shockwaveEffect;

    [SerializeField]
    private GameObject impactEffect;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;

        shockwaveEffect.transform.position = position;
        impactEffect.transform.position = position + new Vector3(0.1f, 0.1f, 0.1f);
        shockwaveEffect.SetActive(true);

        if (!collision.transform.CompareTag("Player"))
        {
            impactEffect.SetActive(true);
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        Destroy(gameObject, 0.75f);
    }
}
