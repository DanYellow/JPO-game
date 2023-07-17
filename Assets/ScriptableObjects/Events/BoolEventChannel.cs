using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "BoolEventChannel", menuName = "ScriptableObjects/Events/BoolEventChannel", order = 0)]
public class BoolEventChannel : ScriptableObject
{
    public UnityAction<bool> OnEventRaised;

	public void Raise(bool value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
