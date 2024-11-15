using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Scriptable Objects"), SerializeField]
    private PlayerData playerData;

    private void Awake()
    {
        GetComponentInChildren<Light>().transform.name = $"Light{playerData.id.ToString()}";

        if (playerData.id == PlayerID.Player2 || playerData.id == PlayerID.Player3)
        {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
    }
}
