using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "VoidEventChannel", menuName = "ScriptableObjects/Events/VoidEventChannel", order = 0)]
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
