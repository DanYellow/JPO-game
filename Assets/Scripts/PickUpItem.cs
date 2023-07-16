using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public CollectibleVariable data;
    
    private Animator animator;
    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {


            gameObject.SetActive(false);
        }
    }
}
