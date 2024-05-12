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

    [HideInInspector]
    public float speedFactor = 1;

    private Vector3 hitTargetDirection = Vector3.zero;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onGameOver;

    [SerializeField]
    private VoidEventChannel onCameraSwitch;

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
        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point;

        shockwaveEffect.transform.position = position;
        shockwaveEffect.SetActive(true);

        if (collision.transform.CompareTag("Ground"))
        {
            Quaternion rot = Quaternion.LookRotation(contact.normal);

            GameObject impactEffect = Instantiate(impactEffectPrefab, position, rot);
            impactEffect.transform.parent = collision.transform;
            impactEffect.transform.localScale = new Vector3(0.0001f, 0.0002f, 0.0001f);
        }
        else if (collision.transform.CompareTag("Player"))
        {
            onGameOver.Raise();
            onCameraSwitch.Raise();
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.enabled = false;

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;

        onCrash?.Invoke();
    }

    private void FixedUpdate()
    {
        if (hitTargetDirection == Vector3.zero)
        {
            hitTargetDirection = (hitTarget.position - transform.position).normalized;
        }

        rb.position += speed * speedFactor * Time.deltaTime * hitTargetDirection;
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
