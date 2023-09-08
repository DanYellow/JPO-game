using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField]
    private PotionValue potionTypeValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }

            playerHealth.Heal(potionTypeValue);
            Destroy(gameObject, 0.05f);
        }
    }
}
