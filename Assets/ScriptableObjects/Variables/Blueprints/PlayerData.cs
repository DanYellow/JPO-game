
using UnityEngine;

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
}