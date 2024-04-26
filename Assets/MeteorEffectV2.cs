using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=yiTF4rJu6tY
public class MeteorEffectV2 : MonoBehaviour
{
    [SerializeField]
    private float speed = 1;

    [SerializeField]
    private float delayBeforeDeath;

    private Vector3 initScale;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onCarDamage;

    [SerializeField]
    private BoolValue isCarTakingDamage;

    private void Awake()
    {
        initScale = transform.localScale;
    }

    private void OnEnable() {
        // gameObject.SetActive(false);
    }

    public void Shockwave() {
        gameObject.SetActive(true);
        StartCoroutine(ResetThyself());
    }

    public IEnumerator ResetThyself()
    {
        yield return new WaitForSeconds(delayBeforeDeath);
        transform.localScale = initScale;
        GetComponentInParent<ObjectPooled>().Release();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !isCarTakingDamage.CurrentValue)
        {
            Vector3 collisionPoint = collision.ClosestPoint(transform.position);
            Rigidbody rb = collision.GetComponent<Rigidbody>();
            rb.AddExplosionForce(10f, collisionPoint, 10, 0f, ForceMode.Impulse);
            onCarDamage.Raise();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 meshScale = transform.localScale;
        float growth = speed * Time.deltaTime;
        transform.localScale = new Vector3(
            meshScale.x + growth,
            meshScale.y + growth,
            meshScale.z + growth
        );
    }
}
