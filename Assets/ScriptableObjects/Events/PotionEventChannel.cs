using UnityEngine;
using UnityEngine.Events;

public enum PotionType
{
    Heal,
}


[CreateAssetMenu(fileName = "PotionEventChannel", menuName = "ScriptableObjects/Events/PotionEventChannel", order = 0)]
public class PotionEventChannel : ScriptableObject
{
    public PotionType potionType;
    public int value;

    public UnityAction<PotionType, int> OnEventRaised;

    public void Raise()
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(potionType, value);
    }

    [Multiline]
    public string DeveloperDescription = "";
}
