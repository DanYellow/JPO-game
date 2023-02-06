using System.Collections;
using UnityEngine;

public class LaserSprite : MonoBehaviour
{
    [SerializeField]
    private GameObject stretchPart;

    private SpriteRenderer stretchPartSprite;
    private BoxCollider2D stretchPartCollider;

    private ContactPoint2D[] listContacts = new ContactPoint2D[1];

    private float timeToReachTarget = 1.5f;
    public float damage = 0;

    private void Awake()
    {
        stretchPartSprite = stretchPart.GetComponent<SpriteRenderer>();
        stretchPartCollider = stretchPart.GetComponent<BoxCollider2D>();
    }

    IEnumerator Shoot() {
        float timeElapsed = 0f;

        Vector2 endValue = new Vector2(stretchPart.transform.localScale.x + 50f, stretchPart.transform.localScale.y);
        Vector2 startValue = stretchPart.transform.localScale;
        while (timeElapsed < timeToReachTarget)
        {
            stretchPart.transform.localScale = Vector2.Lerp(
                startValue, 
                endValue, 
                timeElapsed / timeToReachTarget
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }

    void ResetScale() {
        stretchPart.transform.localScale = new Vector3(0, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        other.GetContacts(listContacts);
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(damage);
        }

        if (other.transform.TryGetComponent<IPushable>(out IPushable iPushable))
        {
            iPushable.HitDirection(listContacts[0].normal);
        }
    }

    private void OnEnable() {
        StartCoroutine(Shoot());
    }

    private void OnDisable() {
        ResetScale();
    }
}
 