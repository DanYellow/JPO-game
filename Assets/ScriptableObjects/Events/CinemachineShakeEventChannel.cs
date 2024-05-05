using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CinemachineShakeEventChannel", menuName = "ScriptableObjects/Events/CinemachineShakeEventChannel", order = 0)]
public class CinemachineShakeEventChannel : ScriptableObject
{
    public UnityAction<CameraShakeType> OnEventRaised;

	public void Raise(CameraShakeType value)
	{
		if (OnEventRaised != null) {
			OnEventRaised.Invoke(value);
        }
	}

    [Multiline]
    public string DeveloperDescription = "";
}