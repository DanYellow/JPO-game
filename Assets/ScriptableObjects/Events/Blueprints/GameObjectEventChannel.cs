using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "GameObjectEventChannel", menuName = "ScriptableObjects/Events/GameObjectEventChannel", order = 0)]
public class GameObjectEventChannel : ScriptableObject
{
    public UnityAction<GameObject> OnEventRaised;

	public void Raise(GameObject gameObject)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(gameObject);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
