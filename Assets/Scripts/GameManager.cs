using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerDeathEffectPrefab;

    [SerializeField]
    private GameObject gameEndMenuUI;
    [SerializeField]
    private Image winnerImage;
    [SerializeField]
    private Image winnerShadow;
    [SerializeField]
    private TextMeshProUGUI winnerName;

    [Header("Scriptable Objects"), SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    [SerializeField]
    private GameObjectEventChannel onPlayerDeathEvent;

    [SerializeField]
    private VoidEventChannel onGameEndEvent;

    [SerializeField]
    private VoidEventChannel onTimerEndEvent;

    [SerializeField]
    private GameObjectEventChannel onPlayerWinsEvent;

    private int nbPlayers = 4;

    private void Awake()
    {
        gameEndMenuUI.SetActive(false);
    }

    private void OnEnable()
    {
        onPlayerExitEvent.OnEventRaised += OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
        onPlayerWinsEvent.OnEventRaised += DisplayGameWinner;
        onTimerEndEvent.OnEventRaised += OnTimerEnd;
    }

    private void DisplayGameWinner(GameObject playerGO)
    {
        Player player = playerGO.GetComponent<Player>();
        winnerImage.sprite = player.playerData.image;
        winnerShadow.sprite = player.playerData.image;
        winnerName.SetText($"Le <b>{player.playerData.GetName()}</b>\nremporte la partie !");
    }

    private void OnPlayerExit(Vector3 position)
    {
        StartCoroutine(PlayerDeathEffect(position));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
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
            gameEndMenuUI.SetActive(true);
        }
    }

    private void OnTimerEnd()
    {
        print("okoke");
        onGameEndEvent.Raise();
        gameEndMenuUI.SetActive(true);
    }

    private void OnDisable()
    {
        onPlayerExitEvent.OnEventRaised -= OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised -= OnPlayerDeath;
        onPlayerWinsEvent.OnEventRaised -= DisplayGameWinner;
        onTimerEndEvent.OnEventRaised -= OnTimerEnd;
    }
}
