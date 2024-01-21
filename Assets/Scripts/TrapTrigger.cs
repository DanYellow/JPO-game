using UnityEngine;

public class TrapTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject trap;

    BoxCollider2D bc;

    private void Awake() {
        bc = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 trapPosition = new Vector3(
                bc.bounds.center.x, 
                bc.bounds.min.y,
                0
            );
            GameObject spike = Instantiate(trap, trapPosition, trap.transform.rotation);
        }
    }
}
