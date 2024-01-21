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

    private SpriteMask spriteMask;

    // Start is called before the first frame update
    void Awake()
    {
        SpriteRenderer sr = indicator.GetComponent<SpriteRenderer>();
        endPosition = new Vector3(
            transform.position.x,
            transform.position.y + sr.bounds.size.y * 1.25f,
            0
        );
        
        spriteMask = GetComponent<SpriteMask>();
    }

    private void OnEnable()
    {
        timeElapsed = 0;
        indicator.SetActive(true);
        indicatorOriginPosition = Vector3.zero;
        endPosition.x = transform.position.x;
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

    IEnumerator ThrowSpikeRoutine()
    {
        Vector3 spikePosition = new Vector3(
            transform.position.x,
            spriteMask.bounds.min.y,
            0
        );

        yield return new WaitWhile(() => timeElapsed <= animationDuration);

        mechaBossSpike = Instantiate(mechaBossSpikePrefab.gameObject, spikePosition, mechaBossSpikePrefab.transform.rotation);
        mechaBossSpike.GetComponent<SpriteRenderer>().sortingOrder = -2;
        yield return Helpers.GetWait(0.2f);
        indicator.SetActive(false);
        mechaBossSpike.GetComponent<MechaBossSpike>().Throw(Vector3.up);

    }
}
