using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    public UnityEvent OnBegin, OnDone;

    private float delay = .2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Apply(GameObject target, Vector2 _direction)
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - target.transform.position).normalized;
        // rb.AddForce(direction * strength);
        rb.velocity = new Vector2(direction.x * _direction.x, _direction.y);
        StartCoroutine(Reset());
    }

    public void Apply(GameObject target, int strength)
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
        OnBegin?.Invoke();
        Vector2 direction = (transform.position - target.transform.position).normalized;
        rb.velocity = new Vector2(direction.x * strength, rb.velocity.y);
        StartCoroutine(Reset());
    }

    private IEnumerator Reset()
    {
        yield return Helpers.GetWait(delay);
        rb.velocity = Vector2.zero;
        OnDone?.Invoke();
    }
}
