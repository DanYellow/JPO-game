using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PlayerIDEventChannel", menuName = "ScriptableObjects/Events/PlayerIDEventChannel", order = 0)]
public class PlayerIDEventChannel : ScriptableObject
{
    public UnityAction<PlayerID> OnEventRaised;

	public void Raise(PlayerID playerID)
	{
		if (OnEventRaised != null)
			OnEventRaised.Invoke(playerID);
	}

    [Multiline]
    public string DeveloperDescription = "";
}
