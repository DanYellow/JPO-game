using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public CollectibleVariable data;

    private Animator animator;
    private SpriteRenderer sr;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        transform.position = new Vector3(
            Random.Range(
                ScreenUtility.Instance.Left + (sr.bounds.size.x / 2),
                ScreenUtility.Instance.Right - (sr.bounds.size.x / 2)
            ),
            -4.6f,
            transform.position.z
        );
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("IsPicked");
            StartCoroutine(Disable());
        }
    }

    IEnumerator Disable()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }
}
