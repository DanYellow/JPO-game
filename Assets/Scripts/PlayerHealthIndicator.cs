using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;


public class PlayerHealthIndicator : MonoBehaviour
{
    private Dictionary<PlayerID, string> playerNameDict = new Dictionary<PlayerID, string>(){
        {PlayerID.Player1, "Joueur 1"},
        {PlayerID.Player2, "Joueur 2"},
        {PlayerID.Player3, "Joueur 3"},
        {PlayerID.Player4, "Joueur 4"}
    };

    [SerializeField]
    private Image playerImage;
    [SerializeField]
    private TextMeshProUGUI playerName;

    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private void Awake()
    {
        playerName.SetText($"{playerNameDict[playerData.id]}");
    }
}
