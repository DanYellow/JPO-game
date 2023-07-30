using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;

    private UnityAction<bool> onPause;
    private new Camera camera;

    private void Awake() {
        camera = GetComponent<Camera>();
    }

    private void OnEnable() {
        onPause = (bool isPaused) => { 
            camera.enabled = !isPaused; 
        };
        onTogglePauseEvent.OnEventRaised += onPause;
    }

    private void OnDisable() {
        onTogglePauseEvent.OnEventRaised -= onPause;
    }
}
