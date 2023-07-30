using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private BoolEventChannel onTogglePauseEvent;
    private UnityAction<bool> onPause;
    private new Camera camera;

    [SerializeField]
    private VoidEventChannel onPlayerDeathVoidEventChannel;
    private UnityAction onPlayerDeathVoid;

    private void Awake()
    {
        camera = GetComponent<Camera>();
    }

    private void OnEnable()
    {
        onPause = (bool isPaused) =>
        {
            camera.enabled = !isPaused;
        };
        onTogglePauseEvent.OnEventRaised += onPause;

        onPlayerDeathVoid = () => { camera.enabled = false; };
        // onPlayerDeathVoidEventChannel.OnEventRaised += onPlayerDeathVoid;
    }

    private void OnDisable()
    {
        onTogglePauseEvent.OnEventRaised -= onPause;
        onPlayerDeathVoidEventChannel.OnEventRaised -= onPlayerDeathVoid;
    }
}
