using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ControlHint : MonoBehaviour
{
    private bool isPlayerInRange = false;

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

    [SerializeField]
    CanvasGroup playerHUDCanvasGroup;

    private Queue<string> listSentences;

    private float defaultLightIntensity;

    private void Awake()
    {
        light = GetComponentInChildren<Light2D>(true);
        light.gameObject.SetActive(false);

        defaultLightIntensity = light.intensity;
    }

    void Start()
    {
        Load();
    }

    private void Update() {
        if(isPlayerInRange) {
            light.intensity = Mathf.Lerp(0.85f, defaultLightIntensity, Mathf.PingPong(Time.time / 2, 1));
            // light.intensity = Math.Clamp(Mathf.Sin(Time.time * 1.5f) * defaultLightIntensity, 0.85f, defaultLightIntensity);
        }
    }

    private void Load()
    {
        listSentences = new Queue<string>();
        listSentences.Clear(); //clear any sentences in the queue

        foreach (string sentence in interactionItemTextValue.listSentences) //for each sentence, enqueue it
        {
            listSentences.Enqueue(sentence.Trim());
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
            onInteractRangeEvent.Raise(true);
            Time.timeScale = 0;
            playerHUDCanvasGroup.alpha = 0.05f;
            onInteract.Raise(listSentences.Dequeue() + "\n<color=#ead8b5>▼</color>");
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
            light.intensity = defaultLightIntensity;
            isPlayerInRange = false;
            light.gameObject.SetActive(false);
            onInteractRangeEvent.Raise(isPlayerInRange);
        }
    }

    private void EndDialogue() {
        Time.timeScale = 1;
        playerHUDCanvasGroup.alpha = 1f;
        onInteractRangeEvent.Raise(false);
        onInteractRangeEvent.Raise(true);
        onPlayerInputMapChange.Raise(ActionMapName.Player);
        Load();
    }

    private void OnDisable()
    {
        onPlayerStartInteractEvent.OnEventRaised -= Display;
        
    }
}
