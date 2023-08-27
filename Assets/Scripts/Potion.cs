using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField]
    private PotionEventChannel onPotionPicked;

    [SerializeField]
    private PotionValue potionTypeValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Collider2D collider in gameObject.GetComponents<Collider2D>())
            {
                collider.enabled = false;
            }
            onPotionPicked.Raise(potionTypeValue);
            Destroy(gameObject, 0.5f);
        }
    }
}
