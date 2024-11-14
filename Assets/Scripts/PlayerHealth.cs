using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private int nbLives = 0;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nbLives = playerData.maxNbLives;
    }


    public void TakeDamage(Vector3 impactPoint)
    {
        nbLives--;

        if (nbLives == 0)
        {
            Die(impactPoint);
        }
    }

    private void Die(Vector3 impactPoint)
    {
        rb.AddForce(impactPoint * 50, ForceMode.VelocityChange);
        // rb.AddExplosionForce(1000, impactPoint, 10, 0);
    }
}
