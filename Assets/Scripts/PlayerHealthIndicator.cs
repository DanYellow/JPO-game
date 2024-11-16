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
    }

    private void OnEnable()
    {
        onPlayerHitEvent.OnEventRaised += OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
    }

    private void OnPlayerHit(PlayerID playerID)
    {
        if (playerID == playerData.id)
        {
            print("Player " + playerID.ToString() + " - " + playerData.id);
        }
    }

    private void OnPlayerDeath(GameObject _player)
    {
        Player player = _player.GetComponent<Player>();

        if (player.playerData.id == playerData.id)
        {
            playerImage.material = blackAndWhiteMaterial;
        }
    }

    private void OnDisable()
    {
        onPlayerHitEvent.OnEventRaised += OnPlayerHit;
        onPlayerDeathEvent.OnEventRaised += OnPlayerDeath;
    }

}
