using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=IGDrF1Cq9Q0

public class MechaBossSpike : MonoBehaviour
{
    [SerializeField]
    ProjectileData projectileData;

    public bool throwing = false;

    private SpriteRenderer sr;

    public Vector3 throwDir;

    public Quaternion origRotation { private set; get; }

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        origRotation = transform.rotation;
    }

    private void Update()
    {
        if (throwing)
        {
            transform.position += projectileData.speed * Time.deltaTime * -transform.right;
            // transform.position += projectileData.speed * Time.deltaTime * throwDir;
        }
    }

    public void Throw(Vector3? rotateDir = null)
    {
        rotateDir ??= Vector3.up;
        sr.color = Color.red;
        throwing = true;
        var target = GameObject.Find("Player").transform;
        throwDir = (GameObject.Find("Player").transform.position - transform.position).normalized;
        
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position, transform.TransformDirection((Vector3) rotateDir));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    public void Reset()
    {
        throwing = false;
        sr.color = Color.white;
        transform.rotation = origRotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = collision.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(projectileData.damage);
            if (collision.gameObject.TryGetComponent(out Knockback knockback))
            {
                knockback.Apply(gameObject, projectileData.knockbackForce);
            }
        }
    }
}
