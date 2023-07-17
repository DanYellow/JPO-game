using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "MaterialEventChannel", menuName = "ScriptableObjects/Events/MaterialEventChannel", order = 0)]
public class MaterialEventChannel : ScriptableObject
{
    public UnityAction<MaterialChangeValue> OnEventRaised;

	public void Raise(MaterialChangeValue value)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(value);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
