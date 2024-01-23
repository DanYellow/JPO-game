using System.Collections;
using UnityEngine;
public class MechaBossSpikeSpawn : MonoBehaviour
{
    [SerializeField]
    private MechaBossSpike mechaBossSpikePrefab;
    private GameObject mechaBossSpike;

    [SerializeField]
    private GameObject indicator;
    private float timeElapsed = 0;
    private float animationDuration = 0.6f;
    private Vector3 endPosition;
    private Vector3 indicatorOriginPosition;
    private SpriteRenderer srIndicator;
    private SpriteMask spriteMask;

    // System.Diagnostics.Stopwatch st;

    // Start is called before the first frame update
    void Awake()
    {
        srIndicator = indicator.GetComponent<SpriteRenderer>();
        spriteMask = GetComponentInChildren<SpriteMask>();

        // st = new System.Diagnostics.Stopwatch();
    }
    private void OnEnable()
    {
        timeElapsed = 0;
        indicator.SetActive(true);
        indicatorOriginPosition = Vector3.zero;
        
        endPosition = new Vector3(
            transform.position.x,
            spriteMask.bounds.max.y + (srIndicator.bounds.size.y * 0.85f),
            0
        );
        // st.Start();

        indicator.transform.localPosition = indicatorOriginPosition;
        StartCoroutine(ThrowSpikeRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        if (timeElapsed < animationDuration)
        {
            indicator.transform.position = Vector3.Lerp(indicator.transform.position, endPosition, timeElapsed / animationDuration);
            timeElapsed += Time.deltaTime;
        }

        if (mechaBossSpike != null && Vector2.Distance(mechaBossSpike.transform.position, transform.position) > 100)
        {
            Destroy(mechaBossSpike);
            gameObject.SetActive(false);
        }
    }

    public void DestroyChild() {
        Destroy(mechaBossSpike);
    }

    IEnumerator ThrowSpikeRoutine()
    {
        Vector3 spikePosition = new Vector3(
            transform.position.x,
            spriteMask.bounds.min.y + (mechaBossSpikePrefab.GetComponent<SpriteRenderer>().bounds.size.y / 2),
            0
        );

        yield return new WaitWhile(() => timeElapsed <= animationDuration);

        mechaBossSpike = Instantiate(mechaBossSpikePrefab.gameObject, spikePosition, mechaBossSpikePrefab.transform.rotation);
        mechaBossSpike.GetComponent<SpriteRenderer>().sortingOrder = -2;
        mechaBossSpike.GetComponent<Collider2D>().enabled = false;
        // st.Stop();
        // Debug.Log(string.Format("MyMethod took {0} ms to complete", st.ElapsedMilliseconds));
        yield return Helpers.GetWait(0.15f);
        indicator.SetActive(false);
        mechaBossSpike.GetComponent<Collider2D>().enabled = true;
        mechaBossSpike.GetComponent<MechaBossSpike>().Throw(Vector3.up);
    }
}
