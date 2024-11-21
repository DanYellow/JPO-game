using UnityEngine;
using UnityEngine.UI;

public class UIButtonStartGame : MonoBehaviour
{
    private int nbPlayersReady = 0;
    [Header("Scriptable Objects"), SerializeField]
    private VoidEventChannel onPlayerInputReadyEvent;

    private Button button;

    private void Awake() {
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
        button.interactable = nbPlayersReady >= 1;
    }

    private void OnDisable()
    {
        onPlayerInputReadyEvent.OnEventRaised -= OnPlayerInputReady;
    }
}
