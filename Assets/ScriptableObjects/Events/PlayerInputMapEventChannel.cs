using UnityEngine;
using UnityEngine.Events;

public class ActionMapName
{
    public static string Player = "Player";
    public static string UI = "UI";
    public static string UIGameOverAndCredits = "UIGameOverAndCredits";
    public static string Cinematics = "Cinematics";
    public static string Interact = "Interact";
}

[CreateAssetMenu(fileName = "PlayerInputMapEventChannel", menuName = "ScriptableObjects/Events/PlayerInputMapEventChannel", order = 0)]
public class PlayerInputMapEventChannel : ScriptableObject
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
