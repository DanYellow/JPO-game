using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public CollectibleVariable data;

    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField]
    private FloatValue timeBarValue;

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

    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timeBarValue.CurrentValue = Mathf.Clamp01(timeBarValue.CurrentValue + data.value);
            animator.SetTrigger("IsPicked");

            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            gameObject.SetActive(false);
        }
    }
}
