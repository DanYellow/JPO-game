using System.Collections;
using UnityEngine;

public class LaserSprite : MonoBehaviour
{
    [SerializeField]
    private GameObject stretchPart;

    private SpriteRenderer stretchPartSprite;
    private BoxCollider2D stretchPartCollider;

    float t = 0;
    float timeToReachTarget = 1f;

    private void Awake()
    {
        stretchPartSprite = stretchPart.GetComponent<SpriteRenderer>();
        stretchPartCollider = stretchPart.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        stretchPartCollider.size = new Vector2(
            stretchPartSprite.bounds.size.x / transform.localScale.x,
            stretchPartCollider.size.y
        );

        StartCoroutine(Shoot());
    }

    IEnumerator Shoot() {
        float elapsed = 0f;
        
        while (elapsed < timeToReachTarget)
        {
            stretchPart.transform.localScale += new Vector3(0.25f, 0, 0);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(0);
        }
    }
}
