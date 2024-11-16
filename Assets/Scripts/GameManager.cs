using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerDeathEffectPrefab;

    [Header("Scriptable Objects"), SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    [SerializeField]
    private GameObjectEventChannel onPlayerDeathEvent;

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

    private void OnPlayerDeath(GameObject gameObject)
    {
        nbPlayers--;
        string[] listRankLabel = {
            "1<sup>er</sup>",
            "2<sup>nd</sup>",
            "3<sup>ème</sup>",
            "4<sup>ème</sup>"
        };
        Canvas rankCanvas = gameObject.GetComponent<Player>().rankCanvas;
        TextMeshProUGUI rank = rankCanvas.GetComponentInChildren<TextMeshProUGUI>();
        rank.SetText(listRankLabel[nbPlayers]);
        rankCanvas.gameObject.SetActive(true);

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
