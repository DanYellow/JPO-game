using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    private void OnEnable()
    {
        onPlayerExitEvent.OnEventRaised += OnPlayerExit;
    }

    private void OnPlayerExit(Vector3 position)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        Debug.Log("target is " + screenPos.x + " pixels from the left" + screenPos.y);
    }

    private void OnDisable()
    {


        onPlayerExitEvent.OnEventRaised -= OnPlayerExit;

    }
}
