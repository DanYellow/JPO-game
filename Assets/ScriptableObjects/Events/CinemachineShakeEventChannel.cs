using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CinemachineShakeEventChannel", menuName = "ScriptableObjects/CinemachineShakeEventChannel", order = 0)]
public class CinemachineShakeEventChannel : ScriptableObject
{
    public UnityAction<CameraShakeTypeValue> OnEventRaised;

	public void Raise(CameraShakeTypeValue value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
