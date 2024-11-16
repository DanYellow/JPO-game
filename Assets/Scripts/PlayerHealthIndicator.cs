using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System;

public class PlayerHealthIndicator : MonoBehaviour
{
    private Dictionary<PlayerID, string> playerNameDict = new Dictionary<PlayerID, string>(){
        {PlayerID.Player1, "Joueur 1"},
        {PlayerID.Player2, "Joueur 2"},
        {PlayerID.Player3, "Joueur 3"},
        {PlayerID.Player4, "Joueur 4"}
    };

    [SerializeField]
    private Material blackAndWhiteMaterial;

    [SerializeField]
    private Image playerImage;

    [SerializeField]
    private Image playerImageShadow;

    [SerializeField]
    private TextMeshProUGUI playerName;

    [SerializeField]
    private List<Image> listImages = new List<Image> { };

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private PlayerIDEventChannel onPlayerHitEvent;

    [SerializeField]
    private GameObjectEventChannel onPlayerDeathEvent;

    private void Awake()
    {
        playerName.SetText($"{playerNameDict[playerData.id]}");
        playerImage.sprite = playerData.image;
        playerImageShadow.sprite = playerData.image;

        string filledHeartColor = "#AA0000";

        foreach (var image in listImages)
        {
            if (ColorUtility.TryParseHtmlString(filledHeartColor, out Color hexColor))
            {
                image.color = hexColor;
            }
        }
    }

    private void OnEnable()
    {
        onPlayerHitEvent.OnEventRaised += OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
    }

    private void OnPlayerHit(PlayerID playerID)
    {
        if (playerID == playerData.id && playerData.nbLives > 0)
        {
            listImages[playerData.nbLives].color = Color.white;
        }
    }

    private void OnPlayerDeath(GameObject _player)
    {
        Player player = _player.GetComponent<Player>();

        if (player.playerData.id == playerData.id && playerImage.material != blackAndWhiteMaterial)
        {
            playerImage.material = blackAndWhiteMaterial;
            listImages[0].color = Color.white;
        }
    }

    private void OnDisable()
    {
        onPlayerHitEvent.OnEventRaised += OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
    }
}
