using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    private bool isOpened = false;

    [SerializeField]
    private bool disableAfterAction = false;

    private Animator animator;

    [SerializeField]
    private VoidEventChannel OnOpen;

    [SerializeField]
    private VoidEventChannel OnClose;

    [SerializeField]
    private Collider2D bc2d;

    // private void Awake()
    // {
    //     bc2d = GetComponent<Collider2D>();
    //     animator = GetComponent<Animator>();
    //     if (isOpened)
    //     {
    //         animator.SetTrigger("DoorOpened");
    //     }
    // }

    // private void OnTriggerEnter2D(Collider2D other)
    // {
    //     // if (other.CompareTag("Player"))
    //     // {
    //     //     if (isOpened)
    //     //     {
    //     //         StartCoroutine(Close());
    //     //     }
    //     //     else
    //     //     {
    //     //         StartCoroutine(Open());
    //     //     }
    //     // }
    // }

    // public IEnumerator Open()
    // {
    //     animator.SetBool("IsOpening", true);
    //     bc2d.enabled = false;
    //     yield return null;
    //     yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 1f);
    //     OnOpen.Raise();
    // }

    // public IEnumerator Close()
    // {
    //     animator.SetBool("IsOpening", false);
    //     bc2d.enabled = false;
    //     yield return null;
    //     yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 1f);

    //     OnClose.Raise();
    // }
}
