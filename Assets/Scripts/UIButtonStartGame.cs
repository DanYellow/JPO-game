using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIButtonStartGame : MonoBehaviour
{
    private int nbPlayersReady = 0;
    [Header("Scriptable Objects"), SerializeField]
    private VoidEventChannel onPlayerInputReadyEvent;

    private Button button;

    [SerializeField]
    private UnityEvent onActivate;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.interactable = false;
    }
    private void OnEnable()
    {
        onPlayerInputReadyEvent.OnEventRaised += OnPlayerInputReady;
    }

    private void OnPlayerInputReady()
    {
        nbPlayersReady++;
        if (nbPlayersReady >= 1)
        {
            onActivate.Invoke();
        }
    }

    private void OnDisable()
    {
        onPlayerInputReadyEvent.OnEventRaised -= OnPlayerInputReady;
    }
}
