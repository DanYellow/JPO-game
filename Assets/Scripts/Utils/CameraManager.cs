using Cinemachine;
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

    [SerializeField]
    private BoolEventChannel onToggleCinemachineEventChannel;

    private CinemachineVirtualCamera[] listVCam;

    private void Awake()
    {
        listVCam = FindObjectsOfType<CinemachineVirtualCamera>();
        camera = GetComponent<Camera>();
    }

    private void ToggleVCam(bool enabled) {
        foreach (var vCam in listVCam)
        {
            vCam.enabled = enabled;
        }
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

        onToggleCinemachineEventChannel.OnEventRaised += ToggleVCam;
    }

    private void OnDisable()
    {
        onTogglePauseEvent.OnEventRaised -= onPause;
        onPlayerDeathVoidEventChannel.OnEventRaised -= onPlayerDeathVoid;
        onToggleCinemachineEventChannel.OnEventRaised -= ToggleVCam;
    }
}
