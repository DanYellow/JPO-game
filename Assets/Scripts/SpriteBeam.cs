using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBeam : MonoBehaviour
{
    private Rigidbody2D rb;
    // Start is called before the first frame update
     void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start() {
        rb.velocity = transform.right * 1.5f;
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Destroy(gameObject);
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            Debug.Log("Damage");
            // iDamageable.TakeDamage(enemyData.damage);
        }
    }
}
