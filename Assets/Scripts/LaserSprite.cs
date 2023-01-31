using System.Collections;
using UnityEngine;

public class LaserSprite : MonoBehaviour
{
    [SerializeField]
    private GameObject stretchPart;

    private SpriteRenderer stretchPartSprite;
    private BoxCollider2D stretchPartCollider;

    float timeToReachTarget = 10f;

    private void Awake()
    {
        stretchPartSprite = stretchPart.GetComponent<SpriteRenderer>();
        stretchPartCollider = stretchPart.GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        // stretchPartCollider.size = new Vector2(
        //     stretchPartSprite.bounds.size.x / transform.localScale.x,
        //     stretchPartCollider.size.y
        // );
    }

    private void Update() {
        // stretchPartCollider.size = new Vector2(
        //     (stretchPartSprite.bounds.size.x / transform.localScale.x) / 2,
        //     stretchPartCollider.size.y
        // );
        // stretchPartCollider.offset = new Vector2 (-(stretchPartSprite.bounds.size.x / 2), 0);
    }

    IEnumerator Shoot() {
        float timeElapsed = 0f;
        
        // while (timeElapsed < timeToReachTarget)
        // {
        //     stretchPart.transform.localScale += new Vector3(0.25f, 0, 0);
        //     // stretchPartCollider.size += new Vector2(0.025f, 0);
        //     timeElapsed += Time.deltaTime;
        //     yield return null;
        // }
        Vector2 endValue = new Vector2(50f, stretchPart.transform.localScale.y);
        while (timeElapsed < timeToReachTarget)
        {
            stretchPart.transform.localScale = Vector2.Lerp(
                stretchPart.transform.localScale, 
                endValue, 
                timeElapsed / timeToReachTarget
            );
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // ResetScale();
    }

    void ResetScale() {
        stretchPart.transform.localScale = new Vector3(0, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(0);
        }
    }

    private void OnEnable() {
        StartCoroutine(Shoot());
    }

    private void OnDisable() {
        ResetScale();
    }
}
