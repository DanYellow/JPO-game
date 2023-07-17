using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NPC : MonoBehaviour
{
    [SerializeField]
    private NPCDialogueValue dialogue;

    [SerializeField]
    public TMP_Text dialogueText;

    private Queue<string> listSentences;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Start()
    {
        listSentences = new Queue<string>();
        dialogueText.SetText("");
        listSentences.Clear(); //clear any sentences in the queue

        foreach (string sentence in dialogue.listSentences) //for each sentence, enqueue it
        {
            listSentences.Enqueue(sentence);
        }
    }

    IEnumerator OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            animator.SetTrigger("OpenDialog");
            if (listSentences.Count == 0)
            {
                yield return null;
            }
            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            dialogueText.SetText(listSentences.Dequeue());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            animator.SetTrigger("EndDialog");
        }
    }
}
