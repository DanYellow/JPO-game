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

    [SerializeField]
    private GameObject nextSentenceSprite;

    private bool isPlayerInRange = false;
    private bool isTyping = false;

    private string currentSentence = "";

    [SerializeField]
    private VoidEventChannel playerListenEventChannel;

    [SerializeField]
    private VoidEventChannel endDialogueCallback;

    private bool dialogueHasStarted = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerListenEventChannel.OnEventRaised += DisplayNextSentence;
        nextSentenceSprite.SetActive(false);
    }

    void Start()
    {
        Load();
    }

    private void Load()
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
            // animator.SetLayerWeight(1, 0);
            if (listSentences.Count == 0)
            {
                yield return null;
            }
            animator.SetTrigger("OpenDialog");
            if (dialogueHasStarted)
            {
                nextSentenceSprite.SetActive(true);
            }
            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            isPlayerInRange = true;
            if (!dialogueHasStarted)
            {
                DisplayNextSentence();
            }
        }
    }

    public void DisplayNextSentence()
    {
        dialogueHasStarted = true;
        nextSentenceSprite.SetActive(false);
        
        if ((listSentences.Count == 0 && !isTyping) || !isPlayerInRange)
        {
            EndDialogue();
            return;
        }

        if(isTyping) {
            DisplayFullSentence();
        } else {
            GoToNextSentence();
        }
    }

    private void DisplayFullSentence() {
        StopAllCoroutines();
        dialogueText.text = currentSentence;
        EndSentence();
    }

    private void GoToNextSentence() {
        string nextSentence = listSentences.Dequeue();
        currentSentence = nextSentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(nextSentence));
    }

    // https://github.com/TUTOUNITYFR/creer-un-jeu-en-2d-facilement-unity/blob/master/Assets/Scripts/DialogueManager.cs
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        isTyping = true;
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.025f);
        EndSentence();
    }

    private void EndSentence() {
        isTyping = false;
        nextSentenceSprite.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            animator.SetTrigger("EndDialog");
            nextSentenceSprite.SetActive(false);
        }
    }

    private void EndDialogue()
    {
        Load();
        animator.SetTrigger("EndDialog");
        dialogueHasStarted = false;
        nextSentenceSprite.SetActive(false);
        if (endDialogueCallback && isPlayerInRange)
        {
            endDialogueCallback.Raise();
        }
    }


    private void OnValidate()
    {
        if (dialogue.listSentences.Count > 0)
        {
            string nextSentence = dialogue.listSentences[0];
            dialogueText.SetText(nextSentence);
        }
    }

    private void OnDisable()
    {
        playerListenEventChannel.OnEventRaised -= DisplayNextSentence;
    }
}
