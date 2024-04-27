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

    private Rigidbody rb;
    public Transform hitTarget;

    [SerializeField]
    private float speed = 150f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // print("meteor " + collision.transform.name);
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;

        shockwaveEffect.transform.position = position;
        shockwaveEffect.SetActive(true);

        if (collision.transform.CompareTag("Ground"))
        {
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);

            GameObject impactEffect = Instantiate(impactEffectPrefab, position, rot);
            impactEffect.transform.localScale = transform.localScale * 1.05f;
            impactEffect.transform.parent = collision.transform;
            impactEffect.transform.LookAt(collision.transform);
        }
        else if (collision.transform.CompareTag("Tag"))
        {
            Debug.Log("Gameover");
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        onCrash?.Invoke();
    }

    private void FixedUpdate()
    {
        Vector3 dir = (transform.position - hitTarget.position).normalized * -1;
        rb.velocity += speed * Time.deltaTime * dir;
        rb.rotation = Quaternion.identity;
    }

    public void ResetThyself()
    {
        transform.localPosition = Vector3.zero;
        transform.rotation = Quaternion.identity;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = true;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = false;
    }
}
