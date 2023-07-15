using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private Coroutine autoDestroyCoroutine;

    private Rigidbody2D rb;
    private SpriteRenderer sr;

    private float height;

    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();

        height = sr.bounds.size.y;
    }

    public void Initialize()
    {
        rb.velocity = Vector3.zero;
        transform.position = new Vector3(
            Random.Range(ScreenUtility.Instance.Left, ScreenUtility.Instance.Right),
            ScreenUtility.Instance.Top + height,
            transform.position.z
        );
    }

    IEnumerator AutoDestroy(float duration = 0)
    {
        yield return new WaitForSeconds(duration);
        StopCoroutine(autoDestroyCoroutine);
        Initialize();
    }

    public void OnBecameInvisible()
    {
        if(gameObject.activeSelf) {
            autoDestroyCoroutine = StartCoroutine(AutoDestroy(Random.Range(2.25f, 5.75f)));
        }
    }
}
