using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionItemTextValue", menuName = "ScriptableObjects/Values/InteractionItemTextValue", order = 0)]
public class InteractionItemTextValue : ScriptableObject
{
    [SerializeField, TextArea]
    public List<string> listSentences = new List<string>();

    [SerializeField]
    private VoidEventChannel OnConversationEnd;
}