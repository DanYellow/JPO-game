using UnityEngine;

public class CameraBackgroundArea : MonoBehaviour
{
    [SerializeField]
    private GameObject background;

    private void Awake()
    {
        background.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            background.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            background.SetActive(false);
        }
    }
}
