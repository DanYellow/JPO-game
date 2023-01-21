using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateManager : MonoBehaviour, IOpenable
{
    private Collider2D collider2d;
    private Animator[] listGates;
    private IEnumerator autocloseRef;

    private bool isPlayerIn = false;

    private void Awake()
    {
        collider2d = GetComponent<BoxCollider2D>();
        listGates = GetComponentsInChildren<Animator>();
    }

    public void Open()
    {
        StartCoroutine(ToggleOpening(true));
        autocloseRef = Autoclose();
        StartCoroutine(autocloseRef);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StopCoroutine(autocloseRef);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            StartCoroutine(ToggleOpening(false));
        }
    }

    IEnumerator ToggleOpening(bool isOpening)
    {
         if (isOpening)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        else
        {
            Debug.Log("isOpening " + isOpening);
            gameObject.layer = LayerMask.NameToLayer("Default");
        }

        yield return new WaitForSeconds(0.5f);

       

        foreach (Animator animator in listGates)
            animator.SetBool("IsOpen", isOpening);
        collider2d.isTrigger = isOpening;
    }

    IEnumerator Autoclose()
    {
        yield return new WaitForSeconds(3.5f);

        StartCoroutine(ToggleOpening(false));
    }
}
