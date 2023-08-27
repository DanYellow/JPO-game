using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    [SerializeField]
    private PotionEventChannel onPotionPicked;

    [SerializeField]
    private PotionTypeValue potionTypeValue;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            onPotionPicked.Raise(potionTypeValue);
        }
    }
}
