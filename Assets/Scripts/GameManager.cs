using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Linq;

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

    [Header("Scriptable Objects"), SerializeField]
    private VectorEventChannel onPlayerExitEvent;

    [SerializeField]
    private GameObjectEventChannel onPlayerDeathEvent;

    [SerializeField]
    private VoidEventChannel onGameEndEvent;

    [SerializeField]
    private VoidEventChannel onTimerEndEvent;

    [SerializeField]
    private PlayerIDEventChannel onPlayerWinsEvent;

    [SerializeField]
    private List<PlayerData> listPlayers;

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

    private void DisplayGameWinner(PlayerID playerID)
    {
        PlayerData playerData = listPlayers.Where(item => item.id == playerID).First();

        GameObject winnerDisplay = Instantiate(winnerDisplayPrefab);
        winnerDisplay.transform.parent = listWinnersContainer;

        WinnerCard winnerCard = winnerDisplay.GetComponent<WinnerCard>();
        winnerCard.shadow.sprite = playerData.image;
        winnerCard.image.sprite = playerData.image;
        winnerCard.winnerName.SetText($"Le <b>{playerData.GetName()}</b>\nremporte la partie !");

        Player player = playerData.gameObject.GetComponent<Player>();
        Canvas rankCanvas = player.rankCanvas;
        TextMeshProUGUI rank = rankCanvas.GetComponentInChildren<TextMeshProUGUI>();
        rank.SetText("1<sup>er</sup>");
        rankCanvas.gameObject.SetActive(true);
    }

    private void DisplayGameWinner(PlayerData[] listSoleSurvivors)
    {
        foreach (var soleSurvivor in listSoleSurvivors)
        {
            GameObject winnerDisplay = Instantiate(winnerDisplayPrefab);
            winnerDisplay.transform.parent = listWinnersContainer;

            WinnerCard winnerCard = winnerDisplay.GetComponent<WinnerCard>();
            winnerCard.shadow.sprite = soleSurvivor.image;
            winnerCard.image.sprite = soleSurvivor.image;
            winnerCard.winnerName.SetText($"Le <b>{soleSurvivor.GetName()}</b>\nremporte la partie !");

            Player player = soleSurvivor.gameObject.GetComponent<Player>();
            Canvas rankCanvas = player.rankCanvas;
            TextMeshProUGUI rank = rankCanvas.GetComponentInChildren<TextMeshProUGUI>();
            rank.SetText("1<sup>er</sup>");
            rankCanvas.gameObject.SetActive(true);
        }
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
        onGameEndEvent.Raise();
        gameEndMenuUI.SetActive(true);

        int maxLives = listPlayers.Max(item => item.nbLives);

        PlayerData[] listSoleSurvivors = listPlayers.Where(item => item.nbLives == maxLives).ToArray();
        print(listPlayers.Where(item => item.nbLives == maxLives));
        // PlayerData winner = listPlayers.Where(item => item.nbLives == maxLives);
        DisplayGameWinner(listSoleSurvivors);
    }

    private void OnDisable()
    {
        onPlayerExitEvent.OnEventRaised -= OnPlayerExit;
        onPlayerDeathEvent.OnEventRaised -= OnPlayerDeath;
        onPlayerWinsEvent.OnEventRaised -= DisplayGameWinner;
        onTimerEndEvent.OnEventRaised -= OnTimerEnd;
    }
}
