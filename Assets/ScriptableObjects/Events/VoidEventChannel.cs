using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "EndlessRunnerJPO/VoidEventChannel", order = 0)]
public class VoidEventChannel : ScriptableObject
{
    public UnityAction OnEventRaised;

	public void Raise()
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke();
	}

    [Multiline]
    public string DeveloperDescription = "";
}
