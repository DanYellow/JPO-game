using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;


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

    [SerializeField]
    private bool isTyping = false;

    private bool displayLastSentence = false;

    private string currentSentence = "";

    [SerializeField]
    private VoidEventChannel playerListenEventChannel;

    [SerializeField]
    private VoidEventChannel endDialogueCallback;

    private bool dialogueHasStarted = false;

    private Coroutine resetDialogueCo;

    [SerializeField]
    private float delayBeforeReset = 10;

    private Coroutine typeSentenceCo;

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
        dialogueHasStarted = false;
        isTyping = false;
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
            StopTyping();
            isPlayerInRange = true;
            dialogueText.fontStyle = FontStyles.Normal;

            if (listSentences.Count == 0)
            {
                dialogueText.text = dialogue.listSentences[dialogue.listSentences.Count - 1];
            }
            if (listSentences.Count != 0 && dialogueHasStarted)
            {
                dialogueText.text = dialogue.listContinueSentences[UnityEngine.Random.Range(
                    0, dialogue.listContinueSentences.Count
                )];
            }

            // animator.SetTrigger("OpenDialogue");
            animator.SetBool("Open", true);
            nextSentenceSprite.SetActive(true);

            yield return null;
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            if (resetDialogueCo != null)
            {
                StopCoroutine(resetDialogueCo);
            }

            // DisplayNextSentence();
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

        if ((listSentences.Count == 0 && !isTyping && isPlayerInRange) || !isPlayerInRange)
        {
            EndDialogue();
            return;
        }


        if (isTyping)
        {
            Debug.Log("DisplayFullSentence");
            DisplayFullSentence();
        }
        else if (displayLastSentence)
        {
            Debug.Log("displayLastSentence");
            dialogueText.text = currentSentence;
            displayLastSentence = false;
            // listSentences.Dequeue();
            EndSentence();
        }
        else
        {
            Debug.Log("GoToNextSentence");
            GoToNextSentence();
        }
    }

    private void DisplayFullSentence()
    {
        StopTyping();
        dialogueText.text = currentSentence;
        EndSentence();
    }

    private void GoToNextSentence()
    {
        string nextSentence = listSentences.Dequeue();
        currentSentence = nextSentence;
        StopTyping();
        typeSentenceCo = StartCoroutine(TypeSentence(nextSentence));
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

    private void EndSentence()
    {
        isTyping = false;
        if (isPlayerInRange)
        {
            nextSentenceSprite.SetActive(true);
        }
    }

    IEnumerator OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            displayLastSentence = true;
            
            nextSentenceSprite.SetActive(false);

            if (
                listSentences.Count > 0 &&
                dialogue.interruptionSentence != null &&
                !dialogue.listContinueSentences.Contains(dialogueText.text)
            )
            {
                StopTyping();
                dialogueText.fontStyle = FontStyles.Bold;
                yield return typeSentenceCo = StartCoroutine(TypeSentence(dialogue.interruptionSentence));
                yield return new WaitForSeconds(0.75f);
                // listSentences.Dequeue();
            }

            Debug.Log("'ffffee");
            animator.SetBool("Open", false);
            // animator.SetTrigger("EndDialogue");
            // resetDialogueCo = StartCoroutine(ResetDialogue());
        }
        yield return null;
    }

    void StopTyping()
    {
        if (typeSentenceCo != null)
        {
            dialogueText.text = "...";
            StopCoroutine(typeSentenceCo);
        }
    }

    IEnumerator ResetDialogue()
    {
        // animator.SetTrigger("EndDialogue");
        animator.SetBool("Open", false);
        yield return new WaitForSeconds(delayBeforeReset);
        Load();
    }

    private void EndDialogue()
    {
        // animator.SetTrigger("EndDialogue");
        animator.SetBool("Open", false);

        if (endDialogueCallback && isPlayerInRange)
        {
            endDialogueCallback.Raise();
        }

        // Load();
        nextSentenceSprite.SetActive(false);
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
