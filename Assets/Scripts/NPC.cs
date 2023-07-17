using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField]
    // private VoidEventChannel OnConversationEnd;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // OnConversationEnd.Raise();
            Debug.Log("Geeee");
        }
    }
}
