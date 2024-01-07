using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// https://www.youtube.com/watch?v=IGDrF1Cq9Q0

public class MechaBossSpike : MonoBehaviour
{
    [SerializeField]
    ProjectileData projectileData;

    private bool throwing = false;

    private SpriteRenderer sr;

    private Vector3 throwDir;

    private void Awake()
    {
        print("mcsss");
        sr = GetComponent<SpriteRenderer>();   
    }

    private void Update()
    {
        if (throwing)
        {
            transform.position += throwDir * projectileData.speed * Time.deltaTime;
        }
    }

    public void Throw()
    {
        sr.color = Color.red;
        throwing = true;
        var target = GameObject.Find("Player").transform;
        throwDir = (GameObject.Find("Player").transform.position - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(target.position - transform.position, transform.TransformDirection(Vector3.up));
        transform.rotation = new Quaternion(0, 0, rotation.z, rotation.w);
    }

    public void Reset() {
        throwing = false;
        sr.color = Color.white;
    }
 
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            IDamageable iDamageable = other.transform.GetComponentInChildren<IDamageable>();
            iDamageable.TakeDamage(projectileData.damage);
            if (other.gameObject.TryGetComponent(out Knockback knockback))
            {
                knockback.Apply(gameObject, projectileData.knockbackForce);
            }
        }
    }
}
