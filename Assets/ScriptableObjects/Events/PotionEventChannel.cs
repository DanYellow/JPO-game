using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "PotionEventChannel", menuName = "ScriptableObjects/Events/PotionEventChannel", order = 0)]
public class PotionEventChannel : ScriptableObject
{
    public UnityAction<PotionTypeValue> OnEventRaised;

    public void Raise(PotionTypeValue potionTypeValue)
    {
        if (OnEventRaised != null)
            OnEventRaised.Invoke(potionTypeValue);
    }

    [Multiline]
    public string DeveloperDescription = "";
}
