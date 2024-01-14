using UnityEngine;

public class CameraBackgroundArea : MonoBehaviour
{
    [SerializeField]
    private GameObject background;

    [SerializeField]
    private GameObject player;

    private void Awake()
    {
        background.SetActive(false);
        
    }

    private void Update() {
        // Vector3 nextPosition = new Vector3(player.transform.position.x, background.transform.position.y, background.transform.position.z);
        // background.transform.position = nextPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            background.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            background.SetActive(false);
        }
    }
}
