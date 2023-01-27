using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyJumpTrigger : MonoBehaviour
{
    [HideInInspector]
    public GameObject sibling = null;

    public UnityEvent onTrigger;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject == sibling) {
            onTrigger?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
