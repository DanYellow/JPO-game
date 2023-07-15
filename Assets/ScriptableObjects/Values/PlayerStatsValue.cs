
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "New Player Stats", menuName = "EndlessRunnerJPO/PlayerStatsValue", order = 0)]
public class PlayerStatsValue : ScriptableObject
{
    public float moveSpeed;

    [Range(0, 5)]
    public int nbMaxLifes = 3;
    public int nbCurrentLifes;

    [SerializeField]
    public InvulnerableDataValue invulnerableData;

    private void OnEnable() {
        nbCurrentLifes = nbMaxLifes;
    }
}
