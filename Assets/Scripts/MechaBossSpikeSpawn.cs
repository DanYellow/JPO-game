using System.Collections;
using UnityEngine;

public class MechaBossSpikeSpawn : MonoBehaviour
{
    [SerializeField]
    MechaBossSpike mechaBossSpike;

    float timeElapsed;
    float lerpDuration = 1.65f;
    Vector3 endPosition;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ThrowSpike());

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        endPosition = new Vector3(
            transform.position.x,
            transform.position.y + sr.bounds.size.y,
            0
         );
    }

    void Update()
    {
        if (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(transform.position, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
        }
    }

    IEnumerator ThrowSpike()
    {
        Vector3 spikePosition = new Vector3(
            transform.position.x,
            transform.position.y - 2.5f,
            0
        );

        yield return new WaitWhile(() => timeElapsed < lerpDuration);

        GameObject spike = Instantiate(mechaBossSpike.gameObject, spikePosition, mechaBossSpike.transform.rotation);
        spike.GetComponent<SpriteRenderer>().sortingOrder = -2;
        yield return Helpers.GetWait(0.2f);
        Destroy(gameObject);
        spike.GetComponent<MechaBossSpike>().Throw(Vector3.up);
    }
}
