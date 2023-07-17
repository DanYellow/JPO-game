using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogueValue", menuName = "ScriptableObjects/Values/NPCDialogueValue", order = 0)]
public class NPCDialogueValue : ScriptableObject
{
    public Queue<string> dialogueText = new Queue<string>();

    [SerializeField]
    private VoidEventChannel OnConversationEnd;
}