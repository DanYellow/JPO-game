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

    private Vector3 hitTargetDirection = Vector3.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        hitTargetDirection = Vector3.zero;
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
        else if (collision.transform.CompareTag("Player"))
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
        Vector3 dir = (transform.position - hitTarget.position).normalized;

        if (hitTargetDirection == Vector3.zero)
        {
            hitTargetDirection = (hitTarget.position - transform.position).normalized;
            // rb.AddForce(hitTargetDirection * Physics.gravity.y * speed);
        }
        // print(dir);
        // print(hitTargetDirection);
        // print((transform.position - hitTarget.position).normalized);
        rb.position += speed * Time.deltaTime * hitTargetDirection;
        // rb.AddForce(hitTargetDirection * Physics.gravity.y * speed);

        // transform.rotation = Quaternion.FromToRotation(transform.up, hitTargetDirection) * transform.rotation;
        // var rotation = Quaternion.LookRotation(hitTargetDirection);
        // transform.localRotation = Quaternion.Lerp(transform.rotation, rotation, 1);
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
