using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    [SerializeField]
    private VoidEventChannel onPlayerDeathEvent;

    private int nbPlayers = 4;

    private void OnEnable()
    {
        onPlayerExitEvent.OnEventRaised += OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
    }

    private void OnPlayerExit(Vector3 position)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        Debug.Log("target is " + screenPos.x + " pixels from the left" + screenPos.y);
    }

    private void OnPlayerDeath() {
        nbPlayers--;
        if (nbPlayers == 1) {
            print("Game !");
        }
    }

    private void OnDisable()
    {
        onPlayerExitEvent.OnEventRaised -= OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised -= OnPlayerDeath;
    }
}
