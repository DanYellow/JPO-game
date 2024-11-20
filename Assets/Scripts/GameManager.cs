using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject playerDeathEffectPrefab;

    [SerializeField]
    private GameObject gameEndMenuUI;

    [SerializeField]
    private GameObject winnerDisplayPrefab;

    [SerializeField]
    private Transform listWinnersContainer;

    [SerializeField]
    private GameObject loadingCanvas;

    [SerializeField]
    private GameObject letsgoCanvas;

    [SerializeField]
    private List<PlayerData> listPlayers;

    [Header("Scriptable Objects"), SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    [SerializeField]
    private PlayerIDEventChannel onPlayerDeathEvent;

    [SerializeField]
    private VoidEventChannel onTimerEndEvent;

    [SerializeField]
    private VoidEventChannel onPlayerReadyEvent;

    [SerializeField]
    private PlayerIDEventChannel onPlayerWinsEvent;

    [SerializeField]
    private VoidEventChannel onGameEndEvent;

    [SerializeField]
    private VoidEventChannel onGameStartEvent;

    private List<PlayerID> listWinners = new List<PlayerID>() { };

    private List<int> listNbLivesSolesSurvivors = new List<int>() { };

    private int nbPlayers;
    private int nbPlayersReady;
    private int maxLives = 0;

    private string[] listRankLabel = {
            "1<sup>er</sup>",
            "2<sup>nd</sup>",
            "3<sup>ème</sup>",
            "4<sup>ème</sup>"
        };

    private void Awake()
    {
        nbPlayers = listPlayers.Count();
        gameEndMenuUI.SetActive(false);
        loadingCanvas.SetActive(true);
        letsgoCanvas.SetActive(false);
    }

    private void OnEnable()
    {
        onPlayerExitEvent.OnEventRaised += OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
        onPlayerWinsEvent.OnEventRaised += DisplayGameWinner;
        onTimerEndEvent.OnEventRaised += OnTimerEnd;
        onPlayerReadyEvent.OnEventRaised += OnPlayerReady;
    }

    private void DisplayGameWinner(PlayerID playerID)
    {
        if (listWinners.Contains(playerID))
        {
            return;
        }

        PlayerData playerData = listPlayers.Where(item => item.id == playerID).First();
        Player player = playerData.gameObject.GetComponent<Player>();
        Canvas rankCanvas = player.rankCanvas;
        TextMeshProUGUI rank = rankCanvas.GetComponentInChildren<TextMeshProUGUI>();

        if (listNbLivesSolesSurvivors.Count == 0)
        {
            rank.SetText(listRankLabel[0]);
        }
        else
        {
            int indexPodium = listNbLivesSolesSurvivors.FindIndex(item => item == playerData.nbLives);
            rank.SetText(listRankLabel[indexPodium]);
        }

        rankCanvas.gameObject.SetActive(true);

        if (maxLives != 0 && playerData.nbLives < maxLives)
        {
            return;
        }

        GameObject winnerDisplay = Instantiate(winnerDisplayPrefab);
        winnerDisplay.transform.parent = listWinnersContainer;
        winnerDisplay.transform.localScale = Vector3.one;

        WinnerCard winnerCard = winnerDisplay.GetComponent<WinnerCard>();
        winnerCard.shadow.sprite = playerData.image;
        winnerCard.image.sprite = playerData.image;
        winnerCard.winnerName.SetText($"Le <b>{playerData.GetName()}</b>\nremporte la partie !");

        listWinners.Add(playerID);
    }

    private void OnPlayerExit(Vector3 position)
    {
        StartCoroutine(PlayerDeathEffect(position));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoBackToHome()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator PlayerDeathEffect(Vector3 position)
    {
        GameObject playerDeathEffect = Instantiate(playerDeathEffectPrefab, position, Quaternion.identity);
        yield return Helpers.GetWait(playerDeathEffect.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
        Destroy(playerDeathEffect);
    }

    private void OnPlayerDeath(PlayerID playerID)
    {
        nbPlayers--;

        PlayerData playerData = listPlayers.Where(item => item.id == playerID).First();

        Canvas rankCanvas = playerData.gameObject.GetComponent<Player>().rankCanvas;
        TextMeshProUGUI rank = rankCanvas.GetComponentInChildren<TextMeshProUGUI>();
        rank.SetText(listRankLabel[nbPlayers]);
        rankCanvas.gameObject.SetActive(true);

        if (nbPlayers == 1)
        {
            onGameEndEvent.Raise();
            gameEndMenuUI.SetActive(true);
            gameEndMenuUI.GetComponentInChildren<Button>().Select();
        }
    }

    private void OnTimerEnd()
    {
        maxLives = listPlayers.Max(item => item.nbLives);
        listNbLivesSolesSurvivors = listPlayers.OrderByDescending(item => item.nbLives).Select((item) => item.nbLives).Distinct().ToList();

        onGameEndEvent.Raise();
        gameEndMenuUI.SetActive(true);
        gameEndMenuUI.GetComponentInChildren<Button>().Select();
    }

    private void OnDisable()
    {
        onPlayerExitEvent.OnEventRaised -= OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised -= OnPlayerDeath;
        onPlayerWinsEvent.OnEventRaised -= DisplayGameWinner;
        onTimerEndEvent.OnEventRaised -= OnTimerEnd;
        onPlayerReadyEvent.OnEventRaised -= OnPlayerReady;
    }

    private void OnPlayerReady()
    {
        nbPlayersReady++;
        if (nbPlayersReady == nbPlayers)
        {
            onGameStartEvent.Raise();
            loadingCanvas.SetActive(false);
            StartCoroutine(HideLetgoCanvas());
        }
    }

    private IEnumerator HideLetgoCanvas()
    {
        letsgoCanvas.SetActive(true);
        yield return Helpers.GetWait(1.15f);
        letsgoCanvas.SetActive(false);
    }
}
