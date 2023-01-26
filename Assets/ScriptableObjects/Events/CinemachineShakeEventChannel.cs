using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "CinemachineShakeEventChannel", menuName = "EndlessRunnerJPO/CinemachineShakeEventChannel", order = 0)]
public class CinemachineShakeEventChannel : ScriptableObject
{
    public UnityAction<ShakeTypeValue> OnEventRaised;

	public void Raise(ShakeTypeValue value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
