
using UnityEngine;
using System.Collections.Generic;

public enum PlayerID
{
    Player1,
    Player2,
    Player3,
    Player4,
}

[CreateAssetMenu(fileName = "New Player Data", menuName = "ScriptableObjects/Values/PlayerData", order = 0)]
public class PlayerData : ScriptableObject
{
    public BasePlayerData root;
    public LayerMask damageLayer;
    public PlayerID id;
    public int nbLives = 0;

    public Sprite image;
    public string gamerName;

    public string GetName()
    {
        Dictionary<PlayerID, string> playerNameDict = new Dictionary<PlayerID, string>(){
            {PlayerID.Player1, "Joueur 1"},
            {PlayerID.Player2, "Joueur 2"},
            {PlayerID.Player3, "Joueur 3"},
            {PlayerID.Player4, "Joueur 4"}
        };

        return playerNameDict[id];
    }
}