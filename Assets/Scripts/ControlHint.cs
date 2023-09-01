using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class ControlHint : MonoBehaviour
{
    private bool isPlayerInRange = false;
    private bool isStarted = false;

    private new Light2D light;

    [SerializeField]
    private VoidEventChannel onPlayerStartInteractEvent;

    [SerializeField]
    private BoolEventChannel onInteractRangeEvent;

    [SerializeField]
    private StringEventChannel onPlayerInputMapChange;

    [SerializeField]
    private StringEventChannel onInteract;

    [SerializeField]
    private InteractionItemTextValue interactionItemTextValue;

    private Queue<string> listSentences;

    private void Awake()
    {
        light = GetComponentInChildren<Light2D>(true);
        light.gameObject.SetActive(false);
    }

    void Start()
    {
        Load();
    }

    private void Load()
    {
        listSentences = new Queue<string>();
        listSentences.Clear(); //clear any sentences in the queue

        foreach (string sentence in interactionItemTextValue.listSentences) //for each sentence, enqueue it
        {
            listSentences.Enqueue(sentence);
        }
    }

    private void OnEnable()
    {
        onPlayerStartInteractEvent.OnEventRaised += Display;
    }

    private void Display()
    {
        if (isPlayerInRange)
        {
            if(listSentences.Count == 0) {
                EndDialogue();
                return;
            }
            Time.timeScale = 0;
            onInteractRangeEvent.Raise(true);
            onInteract.Raise(listSentences.Dequeue());
            onPlayerInputMapChange.Raise(ActionMapName.Interact);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            light.gameObject.SetActive(isPlayerInRange);
            onInteractRangeEvent.Raise(isPlayerInRange);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            light.gameObject.SetActive(false);
            onInteractRangeEvent.Raise(isPlayerInRange);
        }
    }

    private void EndDialogue() {
        Time.timeScale = 1;
        onInteractRangeEvent.Raise(false);
        onPlayerInputMapChange.Raise(ActionMapName.Player);
        Load();
    }

    private void OnDisable()
    {
        onPlayerStartInteractEvent.OnEventRaised -= Display;
        
    }

    // public void OnSpeak(InputAction.CallbackContext ctx)
    // {
    //     if (ctx.phase == InputActionPhase.Performed && isPlayerInRange)
    //     {
    //         onSpeakEvent.Raise();
    //     }
    // }
}
