using UnityEngine;
using UnityEngine.Events;

public class Meteor : MonoBehaviour
{
    [SerializeField]
    private GameObject shockwaveEffect;

    [SerializeField]
    private GameObject impactEffectPrefab;

    [SerializeField]
    private UnityEvent onCrash;

    private void OnCollisionEnter(Collision collision)
    {
        print("meteor " + collision.transform.name);
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
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        onCrash?.Invoke();

        // Destroy(gameObject, 0.75f);
    }

    public void ResetThyself()
    {
        transform.localPosition = Vector3.zero;
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = false;
    }
}
