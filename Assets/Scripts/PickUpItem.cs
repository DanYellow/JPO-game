using System.Collections;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    public CollectibleVariable data;

    private Animator animator;
    private SpriteRenderer sr;

    [SerializeField]
    private FloatValue timeBarValue;

    private Coroutine autoDisable;

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
        sr.color = Color.white;
        autoDisable = StartCoroutine(AutoDisable());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            timeBarValue.CurrentValue = Mathf.Clamp01(timeBarValue.CurrentValue + data.value);
            StartCoroutine(Disable());
        }
    }

    IEnumerator Disable()
    {
        animator.SetTrigger("IsPicked");
        StopCoroutine(autoDisable);
        yield return null;
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        gameObject.SetActive(false);
    }

    IEnumerator AutoDisable()
    {
        yield return new WaitForSeconds(7);
        sr.color = Color.black;
        StartCoroutine(Disable());

    }
}
