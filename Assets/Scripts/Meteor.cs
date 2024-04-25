using UnityEngine;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private GameObject shockwaveEffect;

    [SerializeField]
    private GameObject impactEffectPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;

        shockwaveEffect.transform.position = position;
        shockwaveEffect.SetActive(true);

        if (!collision.transform.CompareTag("Player"))
        {
            GameObject impactEffect = Instantiate(impactEffectPrefab, position, Quaternion.identity);
            impactEffect.transform.localScale = transform.localScale * 1.05f;
            impactEffect.transform.parent = collision.transform;
            impactEffect.transform.LookAt(collision.transform);
            impactEffect.SetActive(true);
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        Destroy(gameObject, 0.75f);
    }
}
