using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject trapPrefab;
    private GameObject trap;

    private BoxCollider2D bc;

    private bool isActivated = true;
    [SerializeField]
    private float delayBeforeNextTrigger = 3.75f;
    private float timer = 0;

    Vector3 trapPosition;

    [SerializeField]
    private bool activatedOnStay = false;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();

        float size = 0;
        if (trapPrefab.GetComponentInChildren<Collider2D>() != null)
        {
            Collider2D collider = trapPrefab.GetComponentInChildren<Collider2D>();
            size = collider.bounds.size.y / 2;
        }
        else if (trapPrefab.GetComponentInChildren<SpriteMask>() != null)
        {
            SpriteMask spriteMask = trapPrefab.GetComponentInChildren<SpriteMask>();
            size = spriteMask.bounds.size.y / 2;
        }
        else if (trapPrefab.GetComponentInChildren<SpriteRenderer>() != null)
        {
            SpriteRenderer spriteRenderer = trapPrefab.GetComponentInChildren<SpriteRenderer>();
            size = spriteRenderer.bounds.size.y / 2;
        }

        trapPosition = new Vector3(
            bc.bounds.center.x,
            bc.bounds.min.y - size,
            0
        );
        trap = Instantiate(trapPrefab, trapPosition, trapPrefab.transform.rotation);
        trap.SetActive(false);
    }

    private void Update()
    {
        if (!isActivated)
        {
            timer += Time.deltaTime;
            isActivated = timer >= delayBeforeNextTrigger;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActivated)
        {
            isActivated = false;
            timer = 0;
            trapPosition.x = transform.position.x;

            trap.transform.position = trapPosition;
            trap.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActivated && activatedOnStay)
        {
            isActivated = false;
            timer = 0;
            trap.transform.position = trapPosition;
            trap.SetActive(true);
        }
    }
}
