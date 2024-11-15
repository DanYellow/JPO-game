using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerDeathEffectPrefab;

    [Header("Scriptable Objects"), SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    [SerializeField]
    private VoidEventChannel onPlayerDeathEvent;

    [SerializeField]
    private VoidEventChannel onGameEndEvent;

    private int nbPlayers = 4;

    private void OnEnable()
    {
        onPlayerExitEvent.OnEventRaised += OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
    }

    private void OnPlayerExit(Vector3 position)
    {
        StartCoroutine(PlayerDeathEffect(position));
    }

    IEnumerator PlayerDeathEffect(Vector3 position)
    {
        GameObject playerDeathEffect = Instantiate(playerDeathEffectPrefab, position, Quaternion.identity);
        yield return Helpers.GetWait(playerDeathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(playerDeathEffect);
    }

    private void OnPlayerDeath()
    {
        nbPlayers--;
        if (nbPlayers == 1)
        {
            onGameEndEvent.Raise();
        }
    }

    private void OnDisable()
    {
        onPlayerExitEvent.OnEventRaised -= OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised -= OnPlayerDeath;
    }
}
