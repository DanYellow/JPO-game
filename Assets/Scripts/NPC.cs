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

    [SerializeField]
    private GameObject bubble;
    private Animator animatorBubble;

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
    private bool sentenceWasCompleted = false;

    private Coroutine resetDialogueCo;

    [SerializeField]
    private float delayBeforeReset = 10;

    private Coroutine typeSentenceCo;

    private void Awake()
    {
        animatorBubble = bubble.GetComponent<Animator>();
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
            isTyping = false;
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

            animatorBubble.SetBool("OpenBox", true);
            nextSentenceSprite.SetActive(true);

            yield return null;
            yield return new WaitForSeconds(animatorBubble.GetCurrentAnimatorStateInfo(0).length);

            if (resetDialogueCo != null)
            {
                StopCoroutine(resetDialogueCo);
            }

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
            DisplayFullSentence();
        }
        else if (displayLastSentence)
        {
            dialogueText.text = currentSentence;
            dialogueText.ForceMeshUpdate();
            int totalVisibleCharacters = dialogueText.textInfo.characterCount;

            dialogueText.maxVisibleCharacters = totalVisibleCharacters;
            displayLastSentence = false;
            isTyping = false;
            EndSentence();
        }
        else
        {
            GoToNextSentence();
        }
    }

    private void DisplayFullSentence()
    {
        StopTyping();
        dialogueText.text = currentSentence;
        dialogueText.ForceMeshUpdate();
        dialogueText.maxVisibleCharacters = dialogueText.textInfo.characterCount;
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
    // https://www.youtube.com/watch?v=UR_Rh0c4gbY
    // https://www.youtube.com/watch?v=XqjEB9ySUm0
    IEnumerator TypeSentence(string sentence, bool isInterrupted = false)
    {
        isTyping = true;

        if (sentenceWasCompleted && isPlayerInRange || isInterrupted)
        {
            dialogueText.text = sentence;
        }
        else
        {
            dialogueText.text = currentSentence;
        }

        dialogueText.ForceMeshUpdate();
        int totalVisibleCharacters = dialogueText.textInfo.characterCount;

        dialogueText.maxVisibleCharacters = 0;
        int counter = 0;

        TMP_TextInfo textInfo = dialogueText.textInfo;

        WaitForSeconds internalTypingChar = new WaitForSeconds(0.03f);
        while (counter < totalVisibleCharacters)
        {
            dialogueText.maxVisibleCharacters++;

            yield return internalTypingChar;
            counter++;
        }
        sentenceWasCompleted = true;
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
            isTyping = false;

            nextSentenceSprite.SetActive(false);

            if (
                listSentences.Count > 0 &&
                dialogue.interruptionSentence != null &&
                !dialogue.listContinueSentences.Contains(dialogueText.text)
            )
            {
                StopTyping();
                sentenceWasCompleted = false;
                dialogueText.fontStyle = FontStyles.Bold;
                yield return typeSentenceCo = StartCoroutine(TypeSentence(dialogue.interruptionSentence, true));
                yield return new WaitForSeconds(0.75f);
            }
            resetDialogueCo = StartCoroutine(ResetDialogue());
        }
        yield return null;
    }

    void StopTyping()
    {
        if (typeSentenceCo != null)
        {
            StopCoroutine(typeSentenceCo);
        }
    }

    IEnumerator ResetDialogue()
    {
        animatorBubble.SetBool("OpenBox", false);
        yield return new WaitForSeconds(delayBeforeReset);
        Load();
    }

    private void EndDialogue()
    {
        animatorBubble.SetBool("OpenBox", false);

        if (endDialogueCallback && isPlayerInRange)
        {
            endDialogueCallback.Raise();
        }

        Load();
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
