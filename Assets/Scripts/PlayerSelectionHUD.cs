using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionHUD : MonoBehaviour
{
    [SerializeField]
    private PlayerData playerData;


    [SerializeField]
    private Image playerImage;

    [SerializeField]
    private TextMeshProUGUI playerName;

    private void Awake()
    {
        playerName.SetText($"{playerData.GetName()}");
        playerImage.sprite = playerData.image;
    }

}
