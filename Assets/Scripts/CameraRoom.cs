using UnityEngine;

public class CameraRoom : MonoBehaviour
{
    public new GameObject camera;

    private void Awake() {
        camera.SetActive(false);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            camera.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player")) {
            camera.SetActive(true);
        }
    }
}
