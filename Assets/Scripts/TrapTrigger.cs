using System.Collections;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject trapPrefab;
    private GameObject trap;

    private BoxCollider2D bc;

    private bool isActivated = true;
    [SerializeField]
    private float delayBeforeNextTrigger = 4.75f;
    private float timer = 0;

    Vector3 trapPosition;

    private void Awake()
    {
        bc = GetComponent<BoxCollider2D>();
        trapPosition = new Vector3(
            bc.bounds.center.x,
            bc.bounds.min.y,
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

        // if(Vector2.Distance(trap.transform.position, transform.position) > 100 && trap.activeSelf) {
        //     trap.SetActive(false);
        // }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && isActivated)
        {
            isActivated = false;
            timer = 0;
            trap.transform.position = trapPosition;
            trap.SetActive(true);
            // trap.GetComponent<MechaBossSpikeSpawn>().Trigger();
        }
    }
}
