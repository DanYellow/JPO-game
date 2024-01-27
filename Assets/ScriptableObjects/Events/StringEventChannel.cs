using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "StringEventChannel", menuName = "ScriptableObjects/Events/StringEventChannel", order = 0)]
public class StringEventChannel : ScriptableObject
{
    public UnityAction<string> OnEventRaised;

	public void Raise(string value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
