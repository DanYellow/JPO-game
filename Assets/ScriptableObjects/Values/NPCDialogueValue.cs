using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueValue", menuName = "ScriptableObjects/Values/NPCDialogueValue", order = 0)]
public class NPCDialogueValue : ScriptableObject
{
    [SerializeField, TextArea]
    public List<string> listSentences = new List<string>();

    [SerializeField]
    private VoidEventChannel OnConversationEnd;
}