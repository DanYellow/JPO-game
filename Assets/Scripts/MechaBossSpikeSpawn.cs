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
    private float animationDuration = 0.85f;
    private Vector3 endPosition;
    private Vector3 indicatorOriginPosition;

    // Start is called before the first frame update
    void Start()
    {
        // StartCoroutine(ThrowSpike());

        SpriteRenderer sr = indicator.GetComponent<SpriteRenderer>();
        endPosition = new Vector3(
            transform.position.x,
            transform.position.y + sr.bounds.size.y,
            0
        );
        indicatorOriginPosition = indicator.transform.localPosition;
    }

    private void OnEnable()
    {
        timeElapsed = 0;
        indicator.SetActive(true);
        indicator.transform.localPosition = indicatorOriginPosition;
        StartCoroutine(ThrowSpikeRoutine());
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        // mechaBossSpike.disableNotify -= Disable;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
    }

    public void Trigger()
    {
        // StartCoroutine(ThrowSpikeRoutine());
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
            transform.position.y - 2.5f,
            0
        );

        yield return new WaitWhile(() => timeElapsed <= animationDuration);

        mechaBossSpike = Instantiate(mechaBossSpikePrefab.gameObject, spikePosition, mechaBossSpikePrefab.transform.rotation);
        mechaBossSpike.GetComponent<SpriteRenderer>().sortingOrder = -2;
        yield return Helpers.GetWait(0.2f);
        indicator.SetActive(false);
        // Destroy(gameObject);
        mechaBossSpike.GetComponent<MechaBossSpike>().Throw(Vector3.up);

    }
}
