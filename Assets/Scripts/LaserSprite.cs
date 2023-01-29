using System.Collections;
using System.Collections.Generic;
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

    // Start is called before the first frame update
    void Start()
    {

        stretchPartCollider.size = new Vector2(
            stretchPartSprite.bounds.size.x,
            stretchPartCollider.size.y
        );

        StartCoroutine(Shoot());
    }

    // Update is called once per frame
    void Update()
    {
        t += Time.deltaTime / timeToReachTarget;
        // Debug.Log("t " + t);
        // if (t < timeToReachTarget)
        // {
        //     stretchPart.transform.localScale = Vector3.Lerp(
        //         stretchPart.transform.localScale,
        //         new Vector3(stretchPart.transform.localScale.x * 1.05f, stretchPart.transform.localScale.y, stretchPart.transform.localScale.z),
        //         t
        //     );
        // }
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
            Debug.Log("feeeea");
        if (other.transform.TryGetComponent<IDamageable>(out IDamageable iDamageable))
        {
            iDamageable.TakeDamage(0);
        }
    }
}
