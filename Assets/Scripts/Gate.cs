using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour, IOpenable
{
    private Collider2D collider2d;

    private void Awake() {
        collider2d = GetComponent<BoxCollider2D>();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open() {
        foreach (Animator animator in GetComponentsInChildren<Animator>())
            animator.SetTrigger("IsOpen");
        collider2d.enabled = false;
    }

    private void OnCollisionEnter2D(Collision2D other) {
        Debug.Log(other);
    }
}
