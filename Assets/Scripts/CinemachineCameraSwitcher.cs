using UnityEngine;
using Cinemachine;

public class CinemachineCameraSwitcher : MonoBehaviour
{
    [SerializeField]
    CinemachineVirtualCamera vcam1;

    [SerializeField]
    CinemachineVirtualCamera vcam2;

    private bool isVCam1 = true;

    [Header("Scriptable Objects")]
    [SerializeField]
    private VoidEventChannel onEvent;

    private void OnEnable()
    {
        onEvent.OnEventRaised += SwitchCamera;
    }

    private void SwitchCamera()
    {
        if (isVCam1)
        {
            vcam1.Priority = 0;
            vcam2.Priority = 1;
        }
        else
        {
            vcam1.Priority = 1;
            vcam2.Priority = 0;
        }
        isVCam1 = !isVCam1;
    }

    private void OnDisable()
    {
        onEvent.OnEventRaised -= SwitchCamera;
    }
}
