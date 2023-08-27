using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueValue", menuName = "ScriptableObjects/Values/NPCDialogueValue", order = 0)]
public class NPCDialogueValue : ScriptableObject
{
    [SerializeField, TextArea]
    public List<string> listSentences = new List<string>();

    [Tooltip("Sentence displayed if the player left the NPC before he finishes"), TextArea]
    public string interruptionSentence = "";

    [Tooltip("Sentence displayed if the player left the NPC before he finishes and comes back")]
    public List<string> listContinueSentences = new List<string>();

    [SerializeField]
    private VoidEventChannel OnConversationEnd;
}