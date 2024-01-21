using System.Collections;
using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject trap;

    private BoxCollider2D bc;

    private bool isActivated = true;
    [SerializeField]
    private float delayBeforeNextTrigger = 4.75f;
    private float timer = 0;

    private void Awake() {
        bc = GetComponent<BoxCollider2D>();
    }

    private void Update() {
        if(!isActivated) {
            timer += Time.deltaTime;
            isActivated = timer >= delayBeforeNextTrigger; 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player") && isActivated)
        {
            isActivated = false;
            timer = 0;
            Vector3 trapPosition = new Vector3(
                bc.bounds.center.x, 
                bc.bounds.min.y,
                0
            );
            Instantiate(trap, trapPosition, trap.transform.rotation);
        }
    }
}
